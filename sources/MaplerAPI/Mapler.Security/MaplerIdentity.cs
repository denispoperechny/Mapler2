using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Security
{
    public class MaplerIdentity : IIdentity
    {
        public MaplerIdentity(Guid userId, string login)
        {
            Name = login;
            UserId = userId;
        }

        public string AuthenticationType
        {
            get { return "basic"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name { get; private set; }

        public Guid UserId { get; private set; }
    }
}
