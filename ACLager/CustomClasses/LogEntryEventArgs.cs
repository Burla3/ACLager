using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.CustomClasses
{
    public class LogEntryEventArgs : EventArgs
    {
        public LogEntryEventArgs(string logType, string logBody, string objectData, string objectType) {
            LogType = logType;
            LogBody = logBody;
            ObjectData = objectData;
            ObjectType = objectType;
        }

        public string LogType { get; set; }
        public string LogBody { get; set; }
        public string ObjectData { get; set; }
        public string ObjectType { get; set; }
    }
}