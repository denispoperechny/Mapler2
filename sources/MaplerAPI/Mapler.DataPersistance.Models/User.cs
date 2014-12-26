using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace Mapler.DataPersistance.Models
{
    public class User : IPersistentModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PassHash { get; set; }

        [Required]
        public DateTime Created { get; set; }

    }
}
