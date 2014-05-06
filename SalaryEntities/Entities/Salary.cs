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

        public int CycleYear { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [ForeignKey("FacultyType")]
        public int FacultyTypeId { get; set; }

        [ForeignKey("RankType")]
        public int RankTypeId { get; set; }

        [ForeignKey("AppointmentType")]
        public int AppointmentTypeId { get; set; }

        [ForeignKey("Person")]
        public string PersonID { get; set; }
        
        [Max(1.00)]
        public decimal FullTimeEquivalent { get; set; }

        [Min(0.00)]
        public decimal BannerBaseAmount { get; }

        [Min(0.00)]
        public decimal BaseAmount { get; set; }

        [Min(0.00)]
        public decimal AdminAmount { get; set; }

        [Min(0.00)]
        public decimal EminentAmount { get; set; }

        [Min(0.00)]
        public decimal PromotionAmount { get; set; }

        [Required]
        [Min(0.00)]
        [Display(Name = "Merit Increase", ShortName = "Merit")]
        public decimal MeritIncrease { get; set; }

        [Required]
        [Min(0.00)]
        [Display(Name = "Special Increase", ShortName = "Special")]
        public decimal SpecialIncrease { get; set; }

        [Required]
        [Min(0.00)]
        [Display(Name = "Eminent Increase", ShortName = "Eminent")]
        public decimal EminentIncrease { get; set; }

        [StringLength(128)]
        public string BaseAdjustReason { get; set; }

        [StringLength(1024)]
        public string BaseAdjustNote { get; set; }

        [StringLength(128)]
        public string MeritAdjustReason { get; set; }

        [StringLength(1024)]
        public string MeritAdjustNote { get; set; }

        [StringLength(1024)]
        public string SpecialAdjustNote { get; set; }

        [StringLength(1024)]
        public string Comments { get; set; }

        public FacultyType FacultyType { get; set; }

        public RankType RankType { get; set; }

        public AppointmentType AppointmentType { get; set; }

        public Person Person { get; set; }

        [Display(Name = "Starting Salary", ShortName = "Starting")]
        public decimal TotalAmount
        {
            get
            {
                return (BaseAmount + AdminAmount + EminentAmount + PromotionAmount);
            }
        }


        [Display(Name = "New Salary", ShortName = "New")]
        public decimal NewTotalAmount
        {
            get
            {
                return (TotalAmount + MeritIncrease + SpecialIncrease + EminentIncrease);
            }
        }

        [Display(Name = "Total Increase")]
        public decimal PercentIncrease
        {
            get
            {
                return Math.Round(((decimal.Divide(NewTotalAmount, TotalAmount) - 1) * 100), 1);
            }
        }

        public decimal MeritPercentIncrease
        {
            get
            {
                return Math.Round((decimal.Divide(MeritIncrease, TotalAmount) * 100), 1);
            }
        }

        public decimal SpecialPercentIncrease
        {
            get
            {
                return Math.Round((decimal.Divide(SpecialIncrease, TotalAmount) * 100), 1);
            }
        }

        public decimal EminentPercentIncrease
        {
            get
            {
                return Math.Round((decimal.Divide(EminentIncrease, TotalAmount) * 100), 1);
            }
        }
    }
}
