using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Exceptions
{
    public class ContactNotFoundException : Exception
    {
        public ContactNotFoundException()
        {

        }
        public ContactNotFoundException(string message) : base(message)
        {

        }
        public ContactNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
        public ContactNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
        {

        }
    }
}
