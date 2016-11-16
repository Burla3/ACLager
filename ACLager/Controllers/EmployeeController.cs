using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.Interfaces;

namespace ACLager.Controllers
{
    public class EmployeeController : Controller, ILoggable
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public IEnumerable<User> GetEmployees()
        {
            throw new NotImplementedException();
        }

        public bool CreateEmployee(string name)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEmployee(long uid)
        {
            throw new NotImplementedException();
        }

        public bool ChangeEmployee(string name, bool isActive, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        public event ChangedEventHandler Changed;
    }
}