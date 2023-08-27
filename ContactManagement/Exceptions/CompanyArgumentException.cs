using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Exceptions
{
    public class CompanyArgumentException:Exception
    {
        public CompanyArgumentException()
        {
                
        }
        public CompanyArgumentException(string message):base(message)
        {

        }
        public CompanyArgumentException(string message,Exception inner):base(message,inner)
        {

        }
        public CompanyArgumentException(System.Runtime.Serialization.SerializationInfo info,System.Runtime.Serialization.StreamingContext  context)
        :base(info,context)
        {

        }
    }
}
