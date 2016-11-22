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
        private readonly ACLagerDatabase db = new ACLagerDatabase();

        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<User> users;

            using (db)
            {
                users = db.UserSet;
            }
                                        
            return View(new UserViewModel(users, new User {IsActive = true}));
        }
        
        public ActionResult CreateUser()
        {
            if (HttpContext.Request.HttpMethod == "GET")
            {
                return View();
            }
            else
            {
                // Validate form data

                User user = new User
                {
                    IsActive = bool.Parse(HttpContext.Request["isActive"]),
                    IsAdmin = bool.Parse(HttpContext.Request["isAdmin"]),
                    Name = HttpContext.Request["name"]
                };

                using (ACLagerDatabase db = new ACLagerDatabase()) {
                    db.UserSet.Add(user);
                    db.SaveChanges();
                }

                return Redirect("/User");
            }
        }

        /// <summary>
        /// Edits a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult EditUser(User user) {
            using (db)
            {
                User dbUser = db.UserSet.Find(user.UID);

                dbUser.IsActive = user.IsActive;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.Name = user.Name;

                db.SaveChanges();
            }

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