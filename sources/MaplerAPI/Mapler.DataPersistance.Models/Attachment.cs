using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace Mapler.DataPersistance.Models
{
    public class Attachment : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public String FileName { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public MapItem MapItem { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
