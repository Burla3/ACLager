using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets all log entries from the database
        /// </summary>
        /// <returns>All log entries from the database</returns>
        public IEnumerable<LogEntry> GetLogEntries()
        {
            throw new NotImplementedException();
        }
    }
}