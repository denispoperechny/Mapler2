using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace Mapler.DataPersistance.Models
{
    public class MapItemComment : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public MapItem MapItem { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public String Text { get; set; }

    }
}
