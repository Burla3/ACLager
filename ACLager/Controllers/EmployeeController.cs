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

        /// <summary>
        /// Gets all employees in an IEnumerable.
        /// </summary>
        /// <returns>All employees in an IEnumerable</returns>
        public IEnumerable<User> GetEmployees()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CreateEmployee(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes an existing employee.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>Returns true if successful.</returns>
        public bool DeleteEmployee(long uid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Changes an employee by updating the name, isActive and isAdmin.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isActive"></param>
        /// <param name="isAdmin"></param>
        /// <returns>Returns true if successful.</returns>
        public bool ChangeEmployee(string name, bool isActive, bool isAdmin)
        {
            throw new NotImplementedException();
        }      
    }
}