using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class Salary : AuditEntityBase
    {
        public long Id { get; set; }

        [Index("IX_PersonAndCycleYear", 1, IsUnique = true)]
        [ForeignKey("Person")]
        public string PersonId { get; set; }

        [Index("IX_PersonAndCycleYear", 2, IsUnique = true)]
        public int CycleYear { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [ForeignKey("FacultyType")]
        public int FacultyTypeId { get; set; }

        [ForeignKey("RankType")]
        public int RankTypeId { get; set; }

        [ForeignKey("AppointmentType")]
        public int AppointmentTypeId { get; set; }
        
        [Max(1.00)]
        public decimal FullTimeEquivalent { get; set; }

        [Min(0.00)]
        public int BannerBaseAmount { get; private set; }

        [Min(0.00)]
        public int BaseAmount { get; set; }

        [Min(0.00)]
        public int AdminAmount { get; set; }

        [Min(0.00)]
        public int EminentAmount { get; set; }

        [Min(0.00)]
        public int PromotionAmount { get; set; }

        [Required]
        [Min(0.00)]
        [Display(Name = "Merit Increase", ShortName = "Merit")]
        public int MeritIncrease { get; set; }

        [Required]
        [Min(0.00)]
        [Display(Name = "Special Increase", ShortName = "Special")]
        public int SpecialIncrease { get; set; }

        [Required]
        [Min(0.00)]
        [Display(Name = "Eminent Increase", ShortName = "Eminent")]
        public int EminentIncrease { get; set; }

        [StringLength(1024)]
        public string BaseSalaryAdjustmentNote { get; set; }

        [ForeignKey("MeritAdjustmentType")]
        public int? MeritAdjustmentTypeId { get; set; }

        [StringLength(1024)]
        public string MeritAdjustmentNote { get; set; }

        [StringLength(1024)]
        public string SpecialAdjustmentNote { get; set; }

        [StringLength(1024)]
        public string Comments { get; set; }

        public FacultyType FacultyType { get; set; }

        public RankType RankType { get; set; }

        public AppointmentType AppointmentType { get; set; }

        public MeritAdjustmentType MeritAdjustmentType { get; set; }

        public Person Person { get; set; }

        public ICollection<BaseSalaryAdjustment> BaseSalaryAdjustments { get; set; }

        public ICollection<SpecialSalaryAdjustment> SpecialSalaryAdjustments { get; set; }

        public ICollection<SalaryModification> Modifications { get; set; }
    }
}
