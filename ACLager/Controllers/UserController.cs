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
        // GET: User
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<User> users;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                users = db.UserSet.ToList();
            }

            return View(new UserViewModel(users, new User()));
        }
        
        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                db.UserSet.Add(user);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edits a User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Redirects to /User.</returns>
        [HttpPost]
        public ActionResult EditUser(User user) {
            using (ACLagerDatabase db = new ACLagerDatabase())
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
             using (ACLagerDatabase db = new ACLagerDatabase())
             {
                db.UserSet.Remove(db.UserSet.Find(uid));
                db.SaveChanges();
            }

            return RedirectToAction("Index");
         }

        private short GenerateUniquePIN()
        {
            Random random = new Random();
            short generatedPin;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                IEnumerable<User> users = db.UserSet;

                do
                {
                    generatedPin = (short)random.Next(1000, 10000);
                } while (users.Any(user => user.PIN == generatedPin));
            }

            return generatedPin;
        }
 
         public event ChangedEventHandler Changed;
    }
}