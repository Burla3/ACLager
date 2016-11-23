using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.ViewModels;

namespace ACLager.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            IEnumerable<LogEntry> logEntries;

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                logEntries = db.LogEntrySet.ToList();
            }

            return View(new LogViewModel(logEntries));
        }
    }
}