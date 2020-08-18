using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridgeData.DBModels
{
    public class ErrorLog
    {
        public long ErrorLogId { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string Exception { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
