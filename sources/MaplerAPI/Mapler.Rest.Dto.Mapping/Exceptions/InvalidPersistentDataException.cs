using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Exceptions
{
    public class InvalidPersistentDataException : MappingException
    {
        public InvalidPersistentDataException(string message) 
            : base(message)
        {
        }

        public InvalidPersistentDataException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
