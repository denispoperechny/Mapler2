using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace Mapler.DataPersistance.Models
{
    public class UserPass : IPersistentModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string PassHash { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public static string GetPassHash(string pass)
        {
            // TODO: update
            return pass;
        }
    }
}
