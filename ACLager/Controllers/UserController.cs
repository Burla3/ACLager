using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        [HttpGet]
        public ActionResult Create() {
            return View(new UserViewModel(null, new User()));
        }

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult Create(User user) {
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                user.PIN = GenerateUniquePIN();
                db.UserSet.Add(user);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Bruger oprettet",
                        $"Brugeren {user.Name} blev oprettet.",
                        new {
                            KontekstBruger = GetContextUser().ToLoggable(),
                            OprettetBruger = user.ToLoggable()
                        }
                     )
            );

            return RedirectToAction("Detailed", new { id = user.UID });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id) {
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
        public ActionResult Edit(User user) {
            User oldUser;
            User newUser;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                User dbUser = db.UserSet.Find(user.UID);
                oldUser = dbUser.ToLoggable();

                dbUser.IsActive = user.IsActive;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.Name = user.Name;

                newUser = dbUser.ToLoggable();

                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Bruger ændret",
                        $"Brugeren {newUser.Name} blev ændret.",
                        new {
                            KontekstBruger = GetContextUser().ToLoggable(),
                            Før = oldUser,
                            Efter = newUser
                        }
                    )
            );

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id) {
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
        public ActionResult Delete(long id) {
            User user;

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                user = db.UserSet.Find(id);
                db.UserSet.Remove(user);
                db.SaveChanges();
            }

            Changed?.Invoke(this,
                    new LogEntryEventArgs(
                        "Bruger slettet",
                        $"Brugeren {user.Name} blev slettet.",
                        new {
                            KontekstBruger = GetContextUser().ToLoggable(),
                            Slettet = user.ToLoggable()
                        }
                    )
            );

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

        public static User GetContextUser() {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["UserInfo"];
            long userID = System.Web.Helpers.Json.Decode(cookie?.Value)["UID"];

            User contextUser;
            using (ACLagerDatabase db = new ACLagerDatabase()) {
                contextUser = db.UserSet.Find(userID);
            }
            return contextUser;
        }

        public event ChangedEventHandler Changed;
    }
}