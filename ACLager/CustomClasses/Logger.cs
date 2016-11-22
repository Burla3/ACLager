using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Controllers;
using ACLager.Interfaces;
using ACLager.Models;

namespace ACLager.CustomClasses
{
    public class Logger
    {
        /// <summary>
        /// Saves a <see cref="LogEntry"/> to the database.
        /// </summary>
        /// <param name="sender">The controller which sends the request to log.</param>
        /// <param name="eventArgs">The information to log.</param>
        protected void CreateLogEntry(object sender, LogEntryEventArgs eventArgs)
        {
            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                Type = eventArgs.LogType,
                LogBody = eventArgs.LogBody
            };

            using (ACLagerDatabase db = new ACLagerDatabase())
            {
                db.LogEntrySet.Add(logEntry);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// A method to subscribe to the Changed event.
        /// </summary>
        /// <param name="loggableController">A controller which use the Changed event</param>
        public void Subcribe(ILoggable loggableController)
        {
            loggableController.Changed += CreateLogEntry;
        }
    }
}