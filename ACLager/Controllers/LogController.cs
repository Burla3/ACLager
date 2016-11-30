using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;
using ACLager.ViewModels;
using Microsoft.Owin.Security.Provider;

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

            return View(new LogViewModel(logEntries, null));
        }

        [HttpGet]
        public ActionResult Detailed(string id) {
            if (id == null) {
                return RedirectToAction("Index");
            }

            LogViewModel logViewModel = new LogViewModel();

            using (ACLagerDatabase db = new ACLagerDatabase()) {
                logViewModel.LogEntry = db.LogEntrySet.Find(Int64.Parse(id));
            }

            return View(logViewModel);
        }
    }
}