using System;
using System.Xml.Linq;
using LendingTreeLib.Common;
using Microsoft.Practices.Unity;

namespace LendingTreeLib.Loggers
{
    public class DatabaseLogger : ILogger
    {
        [Dependency("ConnectionString")]
        public string ConnectionString { get; set; }

        public void Log(XElement xElement, EEventType eEventType)
        {
            DatabaseLoggerDatabase db = new DatabaseLoggerDatabase(ConnectionString);

            db.WebLogEntries.InsertOnSubmit(new WebLogEntry
            {
                Timestamp = DateTime.Now,
                EventTypeId = (int)eEventType,
                EventData = new EventData { Data = xElement }
            });

            db.SubmitChanges();
        }
    }
}
