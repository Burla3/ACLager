using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class EmployeeController : Controller
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

        
    }
}