using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Controllers;
using ACLager.Interfaces;

namespace ACLager.CustomClasses
{
    public class Logger
    {
        protected void CreateLogEntry(object sender, LogEntryEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        public void Subcribe(ILoggable loggableController)
        {
            loggableController.Changed += CreateLogEntry;
        }
    }
}