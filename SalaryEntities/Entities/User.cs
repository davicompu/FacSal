using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class User : AuditEntityBase
    {
        public int Id { get; set; }

        public string Pid { get; set; }

        public ICollection<RoleAssignment> RoleAssignments { get; set; }
    }
}
