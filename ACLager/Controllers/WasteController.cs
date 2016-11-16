using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class WasteController : Controller
    {
        // GET: Waste
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets all waste reports and returns them in a IEnumerable.
        /// </summary>
        /// <returns>All waste reports in an IEnumerable</returns>
        public IEnumerable<WasteReport> GetWasteReports()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new waste report, and sets its amount, work_order, item and created_by properties.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="workOrder"></param>
        /// <param name="item"></param>
        /// <param name="createdBy"></param>
        /// <returns>Returns true if successful.</returns>
        public bool CreateWasteReport(long amount, WorkOrder workOrder, Item item, User createdBy)
        {
            throw new NotImplementedException();
        }
    }
}