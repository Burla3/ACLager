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

        // GET: Employee
        [HttpGet]
        public ActionResult Index() {
            IEnumerable<User> users = from user in _db.Users
                                        select user;

            return View(new UserViewModel(users, new User {is_active = true}));
        }

        [HttpPost]
        public ActionResult CreateUser(User user) {

            //if (!ModelState.IsValid) {
            //    IEnumerable<User> users = from userdb in _db.Users
            //                              select userdb;

            //    return View(new UserViewModel(users, new User()));
            //}

            _db.Users.Add(user);
            _db.SaveChanges();

            return Redirect("/User");
        }

        /// <summary>
        /// Edit a user by updating the name, isActive and isAdmin.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <param name="isAdmin"></param>
        /// <returns>Returns true if successful.</returns>
        [HttpPost]
        public ActionResult EditUser(User user) {
            User dbUser = _db.Users.Find(user.uid);
            dbUser.is_active = user.is_active;
            dbUser.is_admin = user.is_admin;
            dbUser.name = user.name;

            _db.SaveChanges();

            return Redirect("/User");
        }

        /// <summary>
        /// Gets all users in an IEnumerable.
        /// </summary>
        /// <returns>All users in an IEnumerable</returns>
        public IEnumerable<User> GetUsers()
         {
             throw new NotImplementedException();
         }
 
 
         /// <summary>
         /// Deletes an existing user.
         /// </summary>
         /// <param name="uid"></param>
         /// <returns>Returns true if successful.</returns>
         public bool DeleteUser(long uid)
         {
             throw new NotImplementedException();
         }
 
     
 
         public event ChangedEventHandler Changed;
    }
}