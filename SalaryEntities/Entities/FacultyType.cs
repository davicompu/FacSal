using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class FacultyType : AuditEntityBase
    {
        public int Id { get; set; }

        [Required]
        [StringLength(35)]
        public string Name { get; set; }
    }
}
