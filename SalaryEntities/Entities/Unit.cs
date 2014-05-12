using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class Unit : AuditEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [StringLength(1024)]
        public string Name { get; set; }

        public string Sequence { get; set; }

        public ICollection<Department> Departments { get; set; }

        public ICollection<UnitModification> Modifications { get; set; }
    }
}
