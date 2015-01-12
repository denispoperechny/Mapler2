using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto
{
    public class MapItemDto
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorFullName { get; set; }

        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public int Color { get; set; }

        public long Longitude { get; set; }

        public long Latitude { get; set; }

        public List<Guid> TagIds { get; set; }
    }
}
