using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.DataAccess.RepositoryDataFiltering.Exceptions
{
    public class DataAccessRestrictedException : InvalidOperationException
    {
        public DataAccessRestrictedException(Type requestedType, Guid requestedId, string currentUserLogin)
            : base(string.Format("Restricted access to resource: '{0}', Id: '{1}' for user login '{2}'", requestedType, requestedId, currentUserLogin))
        {
        }

        public DataAccessRestrictedException(Type requestedType, Guid requestedId, string currentUserLogin, string messsage)
            : base(string.Format("Restricted access to resource: '{0}', Id: '{1}' for user login '{2}'. {3}", requestedType, requestedId, currentUserLogin, messsage))
        {
        }

        public DataAccessRestrictedException(string message)
            : base(message)
        {
        }
    }
}
