using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Exceptions
{
    public class MappingException : InvalidOperationException
    {
        public MappingException(string message) 
            : base(message)
        {
        }

        public MappingException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
