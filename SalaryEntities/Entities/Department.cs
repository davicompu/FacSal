using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class Department : AuditEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Sequence { get; set; }

        [ForeignKey("Unit")]
        public string UnitId { get; set; }

        public  Unit Unit { get; set; }

        public ICollection<Employment> Employees { get; set; }

        public ICollection<DepartmentModification> Modifications { get; set; }
    }
}
