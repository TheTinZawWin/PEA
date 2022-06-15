using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_Common
{
   public class OperationModel
    {
        public int OperationLogSeq { get; set; }
        public System.DateTime OperationDatetime { get; set; }
        public string UserId { get; set; }
        public string CookieUserId { get; set; }
        public string Url { get; set; }
        public string ActionName { get; set; }
        public string ActionParameter { get; set; }
        public string RemoteAddress { get; set; }
        public string RemoteHost { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
