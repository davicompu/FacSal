using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class DepartmentEmployee
    {
        [Column(Order = 0)]
        [Key]
        [ForeignKey("Person")]
        public string PersonId { get; set; }

        [Column(Order = 1)]
        [Key]
        [ForeignKey("Department")]
        public string DepartmentId { get; set; }

        public Person Person { get; set; }

        public Department Department { get; set; }
    }
}
