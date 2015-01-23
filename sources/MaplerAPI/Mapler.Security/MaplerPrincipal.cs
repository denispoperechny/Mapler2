using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Security
{
    public class MaplerPrincipal : IPrincipal
    {
        public MaplerPrincipal(MaplerIdentity identity, IEnumerable<Guid> companies, Guid? copmanyId, bool isSuperUser = false)
        {
            Identity = identity;
            CompanyId = copmanyId;
            IsSuperUser = isSuperUser;
            Companies = companies.ToList();
        }

        public MaplerIdentity Identity { get; private set; }

        IIdentity IPrincipal.Identity {
            get { return Identity; }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public Guid? CompanyId { get; private set; }

        public List<Guid> Companies { get; private set; }

        public bool IsSuperUser { get; private set; }
    }
}
