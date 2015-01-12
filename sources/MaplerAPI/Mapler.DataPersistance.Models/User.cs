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

        [Required]
        public DateTime Created { get; set; }

        [Required]
        // TODO: Decide where to handle and filter
        public bool IsActive { get; set; }

        public bool IsSuperUser { get; set; }

        public string GetFullName()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }
}
