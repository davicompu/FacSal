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
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Index("IX_PersonAndCycleYear", 1, IsUnique = true, IsClustered = false)]
        [ForeignKey("Person")]
        public string PersonId { get; set; }

        [Index("IX_PersonAndCycleYear", 2, IsUnique = true, IsClustered = false)]
        [Max(2050)]
        [Min(2000)]
        public int CycleYear { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [ForeignKey("FacultyType")]
        public int FacultyTypeId { get; set; }

        [ForeignKey("RankType")]
        public string RankTypeId { get; set; }

        [ForeignKey("AppointmentType")]
        public int AppointmentTypeId { get; set; }

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        
        [Required]
        [Max(1.00)]
        [Min(0.00)]
        public decimal FullTimeEquivalent { get; set; }

        [Min(0)]
        public int? BannerBaseAmount { get; private set; }

        [Max(100000000)]
        [Min(0)]
        public int BaseAmount { get; set; }

        [Max(100000000)]
        [Min(0)]
        public int AdminAmount { get; set; }

        [Max(100000000)]
        [Min(0)]
        public int EminentAmount { get; set; }

        [Max(100000000)]
        [Min(0)]
        public int PromotionAmount { get; set; }

        [Required]
        [Max(100000000)]
        [Min(0)]
        [Display(Name = "Merit Increase", ShortName = "Merit")]
        public int MeritIncrease { get; set; }

        [Required]
        [Max(100000000)]
        [Min(0)]
        [Display(Name = "Special Increase", ShortName = "Special")]
        public int SpecialIncrease { get; set; }

        [Required]
        [Max(100000000)]
        [Min(0)]
        [Display(Name = "Eminent Increase", ShortName = "Eminent")]
        public int EminentIncrease { get; set; }

        [StringLength(1024)]
        public string BaseSalaryAdjustmentNote { get; set; }

        [ForeignKey("MeritAdjustmentType")]
        [Display(Name = "Merit adjustment type")]
        public int MeritAdjustmentTypeId { get; set; }

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

        public LeaveType LeaveType { get; set; }

        public Person Person { get; set; }

        public ICollection<BaseSalaryAdjustment> BaseSalaryAdjustments { get; set; }

        public ICollection<SpecialSalaryAdjustment> SpecialSalaryAdjustments { get; set; }

        public ICollection<SalaryModification> Modifications { get; set; }

        public int TotalAmount
        {
            get
            {
                return BaseAmount + AdminAmount + EminentAmount + PromotionAmount;
            }
        }

        public int NewTotalAmount
        {
            get
            {
                return TotalAmount + MeritIncrease + SpecialIncrease + EminentIncrease;
            }
        }
    }
}
