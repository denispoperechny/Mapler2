using DataPersistence.Facade.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.DataPersistance.Models
{
    public class MapItem : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public Company Company { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public string Name { get; set; }

        public int Color { get; set; }

        [Required]
        public long Longitude { get; set; }

        [Required]
        public long Latitude { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
