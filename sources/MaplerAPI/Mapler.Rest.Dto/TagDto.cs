using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto
{
    public class TagDto
    {
        /// <summary>
        /// Readonly
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Readonly
        /// </summary>
        public Guid CompanyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
