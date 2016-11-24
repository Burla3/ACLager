using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers {
    public class UserController : Controller, ILoggable {
        private string _sectionColor = "yellow";
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
            User user;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                user = db.UserSet.Find(Int64.Parse(id));
            }

            return View("Detailed", new UserViewModel(null, user));
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
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                User dbUser = db.UserSet.Find(user.UID);

                dbUser.IsActive = user.IsActive;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.Name = user.Name;

                db.SaveChanges();
            }

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
        /// <param name="uid"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult DeleteUser(long id) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                db.UserSet.Remove(db.UserSet.Find(id));
                db.SaveChanges();
            }

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