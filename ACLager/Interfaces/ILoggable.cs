using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACLager.CustomClasses;

namespace ACLager.Interfaces
{
    public delegate void ChangedEventHandler(object sender, LogEntryEventArgs eventArgs);

    public interface ILoggable
    {
        event ChangedEventHandler Changed;
    }
}
