using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto
{
    public class CompanyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Guid> UserIds { get; set; }

        public DateTime Created { get; set; }

        public Guid AdministratorId { get; set; }
    }
}
