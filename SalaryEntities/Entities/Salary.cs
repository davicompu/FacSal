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
        [Column(Order = 0)]
        [Key]
        [ForeignKey("Person")]
        public string PersonId { get; set; }

        [Column(Order = 1)]
        [Key]
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

        [StringLength(128)]
        public string BaseAdjustReason { get; set; }

        [StringLength(1024)]
        public string BaseAdjustNote { get; set; }

        [ForeignKey("MeritAdjustmentType")]
        public int MeritAdjustmentTypeId { get; set; }

        [StringLength(1024)]
        public string MeritAdjustNote { get; set; }

        [StringLength(1024)]
        public string SpecialAdjustNote { get; set; }

        [StringLength(1024)]
        public string Comments { get; set; }

        public FacultyType FacultyType { get; set; }

        public RankType RankType { get; set; }

        public AppointmentType AppointmentType { get; set; }

        public MeritAdjustmentType MeritAdjustmentType { get; set; }

        public Person Person { get; set; }

        public ICollection<SpecialSalaryAdjustment> SpecialSalaryAdjustments { get; set; }

        public ICollection<SalaryModification> Modifications { get; set; }

        //[Display(Name = "Current Salary", ShortName = "Current")]
        //public int TotalAmount
        //{
        //    get
        //    {
        //        return (BaseAmount + AdminAmount + EminentAmount + PromotionAmount);
        //    }
        //}


        //[Display(Name = "New Salary", ShortName = "New")]
        //public int NewTotalAmount
        //{
        //    get
        //    {
        //        return (TotalAmount + MeritIncrease + SpecialIncrease + EminentIncrease);
        //    }
        //}

        //[Display(Name = "Total Increase")]
        //public decimal PercentIncrease
        //{
        //    get
        //    {
        //        return Math.Round(((decimal.Divide(NewTotalAmount, TotalAmount) - 1) * 100), 1);
        //    }
        //}

        //public decimal MeritPercentIncrease
        //{
        //    get
        //    {
        //        return Math.Round((decimal.Divide(MeritIncrease, TotalAmount) * 100), 1);
        //    }
        //}

        //public decimal SpecialPercentIncrease
        //{
        //    get
        //    {
        //        return Math.Round((decimal.Divide(SpecialIncrease, TotalAmount) * 100), 1);
        //    }
        //}

        //public decimal EminentPercentIncrease
        //{
        //    get
        //    {
        //        return Math.Round((decimal.Divide(EminentIncrease, TotalAmount) * 100), 1);
        //    }
        //}
    }
}
