using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACLager.Models;

namespace ACLager.Controllers
{
    public class LocationController : Controller
    {
        private readonly ACLagerDatabaseEntities _db = new ACLagerDatabaseEntities();
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
        public bool CreateLocation(string newLocationId)
        {
            //If a location with the same id does not exsist, return null.
            var exsistingId = _db.Locations.Find(newLocationId); 

            if (exsistingId != null)
            {
                //The id already exists, so the creation is terminated.
                //some kind of error handling/message
                return false;
            }

            else
            {
                Location newLocation = new Location
                {
                    id = newLocationId,
                    is_active = true
                };
                _db.Locations.Add(newLocation);
                _db.SaveChanges();
                //returns true for creation completed
                return true;
            }

        }
        /// <summary>
        /// Deletes an existing location
        /// </summary>
        /// <param name="toBeDeletedLocationId">ID of the to be deleted location</param>
        /// <returns>Whether or not the deletion was successful</returns>
        public bool DeleteLocation(string toBeDeletedLocationId)
        {
            //If a location with the same id does not exsist, return null.
            var exsistingId = _db.Locations.Find(toBeDeletedLocationId);

            if (exsistingId == null)
            {
                //Location ID does not exsist, so no location to be deleted.
                return false;
            }
            else
            {
                //location exsists, location removed from database.
                _db.Locations.Remove(exsistingId);
                _db.SaveChanges();
                return true;
            }
        }

        public IEnumerable<Location> GetLocations()
        {
            IEnumerable<Location> locations = _db.Locations.ToList();
            return locations;
        }
    }
}