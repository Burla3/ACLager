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

        // GET: Employee
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
        /// Edit a user by updating the name, isActive and isAdmin.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <param name="isAdmin"></param>
        /// <returns>Returns true if successful.</returns>
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