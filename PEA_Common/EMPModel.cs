using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA_Common
{
   public class EMPModel
    {
        
            public Guid ID { get; set; }
            public string EmployeeID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Department { get; set; }
            public DateTime JointDate { get; set; }
            public string Password { get; set; }
            public bool Active { get; set; }
            public Nullable<System.DateTime> CreatedAt { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime>  UpdatedAt { get; set; }
            public string UpdatedBy { get; set; }
       
    }

    public class UpdateEMPTModel
    {
        public Guid ID { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string JointDate { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }
}
