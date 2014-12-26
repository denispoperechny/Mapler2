using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Exceptions
{
    public class NoMapperAvailableException<T, U> : MappingException
    {
        public NoMapperAvailableException() : base(GetTypeName<T, U>())
        {
        }

        public NoMapperAvailableException(Exception inner)
            : base(GetTypeName<T, U>(), inner)
        {
        }

        private static string GetTypeName<A, B>()
        {
            return string.Format("IDtoMapper<{0}, {1}>", typeof (T), typeof (U));
        }
    }
}
