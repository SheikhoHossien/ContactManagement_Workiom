using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagement.Exceptions
{
    public class ContactArgumentException : Exception
    {
        public ContactArgumentException()
        {

        }
        public ContactArgumentException(string message) : base(message)
        {

        }
        public ContactArgumentException(string message, Exception inner) : base(message, inner)
        {

        }
        public ContactArgumentException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
        {

        }
    }
}
