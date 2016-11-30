using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ACLager.CustomClasses;
using ACLager.CustomClasses.Attributes;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    [AdminOnly]
    public class UserController : Controller, ILoggable {
        public UserController() {
            new Logger().Subcribe(this);
        }

        // GET: User
        [HttpGet]
        public ActionResult Index() {
            IEnumerable<User> users;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                users = db.UserSet.ToList();
            }

            return View(new UserViewModel(users, new User()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }
            User user;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                user = db.UserSet.Find(Int64.Parse(id));
            }

            return View(new UserViewModel(null, user));
        }



        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult CreateUser(User user) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                user.PIN = GenerateUniquePIN();
                db.UserSet.Add(user);
                db.SaveChanges();
            }

            string objectData = System.Web.Helpers.Json.Encode(user);
            string objectType = user.GetType().FullName;


            Changed?.Invoke(this, new LogEntryEventArgs("CreateUser", $"Brugeren {user.Name} blev oprettet.", objectData, objectType));

            return RedirectToAction("Detailed", new {id = user.UID});
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditUser(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            UserViewModel userViewModel = new UserViewModel();
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                User dbUser = db.UserSet.Find(Int64.Parse(id));

                if (dbUser == null) {
                    return RedirectToAction("Index");
                }

                userViewModel.User = dbUser;
            }

            return View(userViewModel);
        }


        /// <summary>
        /// Edits a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult EditUser(User user) {
            object data;
            string objectData;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                User dbUser = db.UserSet.Find(user.UID);

                data = new {oldUser = dbUser, newUser = user};
                objectData = System.Web.Helpers.Json.Encode(data);

                dbUser.IsActive = user.IsActive;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.Name = user.Name;

                db.SaveChanges();
            }
            
            string objectType = user.GetType().FullName;

            Changed?.Invoke(this, new LogEntryEventArgs("EditUser", $"Brugeren {user.Name} blev ændret.", objectData, objectType));

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteUser(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            UserViewModel userViewModel = new UserViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                User dbUser = db.UserSet.Find(Int64.Parse(id));

                if (dbUser == null) {
                    return RedirectToAction("Index");
                }

                userViewModel.User = dbUser;
            }

            return View(userViewModel);
        }

        /// <summary>
        /// Deletes an existing user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult DeleteUser(long id) {

            User user;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                user = db.UserSet.Find(id);
                user.IsDeleted = true;
                db.SaveChanges();
            }

            string objectData = System.Web.Helpers.Json.Encode(user);
            string objectType = user.GetType().FullName;


            Changed?.Invoke(this, new LogEntryEventArgs("EditUser", $"Brugeren {user.Name} blev slettet.", objectData, objectType));


            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private short GenerateUniquePIN() {
            Random random = new Random();
            short generatedPin;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                IEnumerable<User> users = db.UserSet;

                do {
                    generatedPin = (short)random.Next(1000, 10000);
                } while (users.Any(user => user.PIN == generatedPin));
            }

            return generatedPin;
        }

        public event ChangedEventHandler Changed;
    }
}