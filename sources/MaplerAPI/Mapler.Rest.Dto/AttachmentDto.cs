using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public String AuthorName { get; set; }

        public String FileName { get; set; }

        public DateTime Created { get; set; }

        public Guid MapItemId { get; set; }

        public string Description { get; set; }
    }
}
