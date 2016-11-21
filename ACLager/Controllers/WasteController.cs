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
        private readonly ACLagerDatabaseEntities _db = new ACLagerDatabaseEntities();
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
            return _db.WasteReports;
        }

        /// <summary>
        /// Creates a new waste report, and sets its amount, work_order, item and created_by properties.
        /// </summary>
        /// <param name="amount">How much of the given product was wasted</param>
        /// <param name="workOrder">Which workorder the waste is related to</param>
        /// <param name="item">The wasted item</param>
        /// <param name="createdBy">The user who created the wastereport </param>
        /// <returns>Returns true if successful.</returns>
        public void CreateWasteReport(long amount, WorkOrder workOrder, Item item, User createdBy)
        {
            //Some cornercases where this check will result in a wastereport with the same information, but is in fact not a dublicate.
            var exsistingWasteReport =
                (from wastereport in _db.WasteReports
                where wastereport.amount == amount && wastereport.work_order == workOrder.uid 
                && wastereport.item == item.uid && wastereport.created_by == createdBy.uid
                select wastereport).FirstOrDefault();

            if (exsistingWasteReport != null)
            {
                _db.WasteReports.Add(new WasteReport()
                {
                    created_by = createdBy.uid,
                    item = item.uid,
                    work_order = workOrder.uid,
                    amount = amount
                });
                _db.SaveChanges();
            }
            else
            {
                //a similar wastereport seems to be exsisting already, confirm creation
                bool confirmation = true;
                if (confirmation)
                {
                    _db.WasteReports.Add(new WasteReport()
                    {
                        created_by = createdBy.uid,
                        item = item.uid,
                        work_order = workOrder.uid,
                        amount = amount
                    });
                    _db.SaveChanges();
                }

            }

            
        }
    }
}