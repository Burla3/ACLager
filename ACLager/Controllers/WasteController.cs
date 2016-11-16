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

        public IEnumerable<WasteReport> GetWasteReports()
        {
            throw new NotImplementedException();
        }

        public bool CreateWasteReport(long amount, WorkOrder workOrder, Item item, User createdBy)
        {
            throw new NotImplementedException();
        }
    }
}