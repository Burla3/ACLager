﻿using System;
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
                if(db.UserSet.Find(user.Name)!=null)
                {/*/user with same name exsist, confirm creation*/ }
                else
                {
                    db.UserSet.Add(user);
                    db.SaveChanges();
                }
                
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

                if (dbUser!=null)
                {
                    dbUser.IsActive = user.IsActive;
                    dbUser.IsAdmin = user.IsAdmin;
                    dbUser.Name = user.Name;

                    db.SaveChanges();
                }
                else
                {
                    //the user specified does not exsist
                }
                
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
                if (db.UserSet.Find(uid)!=null)
                { 
                    db.UserSet.Remove(db.UserSet.Find(uid));
                    db.SaveChanges();
                }
                else
                {
                    //User with given unique ID does not exsist
                }
            }

            return RedirectToAction("Index");
         }
 
         public event ChangedEventHandler Changed;
    }
}