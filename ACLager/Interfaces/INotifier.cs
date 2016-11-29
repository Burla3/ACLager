using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACLager.CustomClasses;

namespace ACLager.Interfaces {

    public delegate void NotificationEventHandler(object sender, NotificationEventArgs eventArgs);

    public interface INotifier {
        event NotificationEventHandler Notify;
    }
}
