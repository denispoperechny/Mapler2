using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto
{
    public class MapItemCommentDto
    {
        public Guid Id { get; set; }

        public Guid MapItemId { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorFullName { get; set; }

        public DateTime Created { get; set; }

        public String Text { get; set; }
    }
}
