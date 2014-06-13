using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class User : AuditEntityBase
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Pid { get; set; }

        public ICollection<RoleAssignment> RoleAssignments { get; set; }
    }
}
