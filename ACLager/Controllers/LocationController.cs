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
        private readonly ACLagerDatabase db = new ACLagerDatabase();
        // GET: Location
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creates a new location 
        /// </summary>
        /// <param name="location">Location to be created</param>
        /// <returns>Whether or not the creation was successful</returns>
        public bool CreateLocation(Location location)
        {
            using (db)
            {
                if (db.LocationSet.All(l => l.Name != location.Name)) {
                    db.LocationSet.Add(location);
                    db.SaveChanges();

                    return true;
                } else {
                    return false;
                }
            }

        }
        /// <summary>
        /// Deletes an existing location
        /// </summary>
        /// <param name="UID">UID of the to be deleted location</param>
        /// <returns>Whether or not the deletion was successful</returns>
        public bool DeleteLocation(long UID)
        {
            using (db)
            {
                if (db.LocationSet.Any(l => l.UID == UID))
                {
                    Location location = db.LocationSet.Single(l => l.UID == UID);
                    db.LocationSet.Remove(location);
                    db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IEnumerable<Location> GetLocations()
        {
            using (db)
            {
                return db.LocationSet;
            }
        }
    }
}