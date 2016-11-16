using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACLager.Controllers
{
    public class LocationController : Controller
    {
        // GET: Location
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Creates a new location 
        /// </summary>
        /// <param name="newLocationId">ID of the to be created location</param>
        /// <returns>Whether or not the creation was successful</returns>
        public bool CreateLocation(long newLocationId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Deletes an existing location
        /// </summary>
        /// <param name="locationId">ID of the to be deleted location</param>
        /// <returns>Whether or not the deletion was successful</returns>
        public bool DeleteLocation(long locationId)
        {
            throw new NotImplementedException();
        }

    }
}