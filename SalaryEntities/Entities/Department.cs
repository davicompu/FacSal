using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Sequence { get; set; }

        [ForeignKey("Unit")]
        public string UnitID { get; set; }

        public  Unit Unit { get; set; }

        public ICollection<Employment> Employees { get; set; }
    }
}
