using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace Mapler.DataPersistance.Models
{
    public class Company : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<User> Users { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public User Administrator { get; set; }

        public Location DefaultLocation { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
