using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Exceptions
{
    public class CompanyNotFoundException:Exception
    {
        public CompanyNotFoundException()
        {

        }
        public CompanyNotFoundException(string message) : base(message)
        {

        }
        public CompanyNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
        public CompanyNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
        {

        }
    }
}
