using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.CustomClasses
{
    public class LogEntryEventArgs : EventArgs
    {
        public LogEntryEventArgs(string logType, string logBody)
        {
            LogType = logType;
            LogBody = logBody;
        }

        public string LogType { get; set; }
        public string LogBody { get; set; }
    }
}