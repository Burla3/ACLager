using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACLager.Models;

namespace ACLager.ViewModels
{
    public class LogViewModel : BaseViewModel
    {
        public IEnumerable<LogEntry> LogEntries { get; set; }
        public LogEntry LogEntry { get; set; }

        public LogViewModel() {
            base.SelectSectionSpecials("Log");
        }

        public LogViewModel(IEnumerable<LogEntry> logEntries, LogEntry logEntry) : this() {
            LogEntries = logEntries;
            LogEntry = logEntry;
        }
    }
}