using SalaryEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacsalData
{
    public class FacsalDbContext : DbContext
    {
        public FacsalDbContext()
            : base("FacsalDbConnection")
        {}

        public DbSet<MeritAdjustmentType> MeritAdjustmentTypes { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentModification> DepartmentModifications { get; set; }
        public DbSet<Employment> Employments { get; set; }
        public DbSet<FacultyType> FacultyTypes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonModification> PersonModifications { get; set; }
        public DbSet<RankType> RankTypes { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryModification> SalaryModifications { get; set; }
        public DbSet<SpecialAdjustmentType> SpecialAdjustmentTypes { get; set; }
        public DbSet<SpecialSalaryAdjustment> SpecialSalaryAdjustments { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitModification> UnitModifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /*
             * Breeze specific configuration items.
             * EF should only get data when told to do so explicitly.
             * http://www.breezejs.com/documentation/entity-framework-dbcontext/
             */
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
