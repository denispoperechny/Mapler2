using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;
using System.ComponentModel.DataAnnotations;

namespace Mapler.DataPersistance.Models
{
    public class Tag : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Company Company { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
