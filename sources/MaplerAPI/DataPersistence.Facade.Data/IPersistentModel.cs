using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPersistence.Facade.Data
{
    public interface IPersistentModel
    {
        [Key]
        Guid Id { get; set; }

        [Required]
        bool IsActive { get; set; }
    }
}