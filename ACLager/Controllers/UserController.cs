using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    public class UserController : Controller
    {
        private readonly ACLagerDatabaseEntities _db = new ACLagerDatabaseEntities();

        // GET: Employee
        [HttpGet]
        public ActionResult Index() {
            IEnumerable<User> users = from user in _db.Users
                                        select user;

            return View(new UserViewModel(users, new User()));
        }

        [HttpPost]
        public ActionResult Create(User user) {

            //if (!ModelState.IsValid) {
            //    IEnumerable<User> users = from userdb in _db.Users
            //                              select userdb;

            //    return View(new UserViewModel(users, new User()));
            //}

            _db.Users.Add(user);
            _db.SaveChanges();

            return Redirect("/User");
        }
    }
}