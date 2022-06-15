using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PEA_Common
{
  
        [Serializable]
        public class CustomException :
           Exception,
           ISerializable
        {
          
            private CustomExceptionType type;

           
            public CustomException() : base()
            {
            }

         
            public CustomException(CustomExceptionType type) : base()
            {
                this.type = type;
            }

          
            public enum CustomExceptionType
            {
               
                None,

               
                SeesionTimeOut
            }

          
            public CustomExceptionType ErrorType
            {
                get { return this.type; }
            }

            /// <summary>
            /// GetObjectData
            /// </summary>
            /// <param name="info">
            /// SerializationInfo</param>
            /// <param name="context">
            /// StreamingContext</param>
            [SecurityCritical]
            public override void GetObjectData(
                SerializationInfo info,
                StreamingContext context)
            {
                base.GetObjectData(info, context);
            }
        }
}
