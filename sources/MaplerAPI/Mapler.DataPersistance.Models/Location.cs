using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace Mapler.DataPersistance.Models
{
    public class Location : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public long Longitude { get; set; }

        [Required]
        public long Latitude { get; set; }

        [Required]
        public float Zoom { get; set; }

        public bool IsSystemDefault { get; set; }
    }
}
