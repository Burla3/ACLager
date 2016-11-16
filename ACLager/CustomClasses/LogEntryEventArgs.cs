using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.CustomClasses
{
    public class LogEntryEventArgs : EventArgs
    {
        public string LogType { get; set; }
        public string LogBody { get; set; }
    }
}