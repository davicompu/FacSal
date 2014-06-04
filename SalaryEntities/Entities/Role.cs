using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class Role : AuditEntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<RoleAssignment> RoleAssignments { get; set; }
    }
}
