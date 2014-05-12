using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class SpecialSalaryAdjustment
    {
        /// <summary>
        /// Including PersonId because it is part of the composite key
        /// for Salary.
        /// </summary>
        [Column(Order = 0)]
        [Key]
        [ForeignKey("Salary")]
        public string PersonId { get; set; }

        /// <summary>
        /// Including CycleYear because it is part of the composite key
        /// for Salary.
        /// </summary>
        [Column(Order = 1)]
        [Key]
        [ForeignKey("Salary")]
        public int CycleYear { get; set; }

        [Column(Order = 2)]
        [Key]
        [ForeignKey("SpecialAdjustmentType")]
        public int SpecialAdjustmentTypeId { get; set; }

        public Salary Salary { get; set; }

        public SpecialAdjustmentType SpecialAdjustmentType { get; set; }
    }
}
