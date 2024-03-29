﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACLager.CustomClasses
{
    public class LogEntryEventArgs : EventArgs
    {
        public LogEntryEventArgs(string logType, string logBody, object objectData) {
            LogType = logType;
            LogBody = logBody;
            ObjectData = objectData;
        }

        public string LogType { get; set; }
        public string LogBody { get; set; }
        public object ObjectData { get; set; }
    }
}