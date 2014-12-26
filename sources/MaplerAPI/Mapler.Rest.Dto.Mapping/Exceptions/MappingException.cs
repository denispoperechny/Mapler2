using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Exceptions
{
    public abstract class MappingException : InvalidOperationException
    {
        protected MappingException(string message) 
            : base(message)
        {
        }

        protected MappingException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
