using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Interfaces;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    public class UserController : Controller, ILoggable
    {
        private readonly ACLagerDatabaseEntities _db = new ACLagerDatabaseEntities();

        // GET: User
        [HttpGet]
        public ActionResult Index() {
            IEnumerable<User> users = from user in _db.Users
                                        select user;

            return View(new UserViewModel(users, new User {is_active = true}));
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult CreateUser(User user) {
            //#ServerSidedValidation
            //if (!ModelState.IsValid) {
            //    IEnumerable<User> users = from userdb in _db.Users
            //                              select userdb;

            //    return View(new UserViewModel(users, new User()));
            //}

            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edits a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult EditUser(User user) {
            User dbUser = _db.Users.Find(user.uid);
            dbUser.is_active = user.is_active;
            dbUser.is_admin = user.is_admin;
            dbUser.name = user.name;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }
 
         /// <summary>
         /// Deletes an existing user.
         /// </summary>
         /// <param name="uid"></param>
         /// <returns>Redirects to /User.</returns>
         [HttpPost]
         public ActionResult DeleteUser(long uid) {                    
             _db.Users.Remove(_db.Users.Find(uid));
             _db.SaveChanges();

             return RedirectToAction("Index");
         }
 
         public event ChangedEventHandler Changed;
    }
}