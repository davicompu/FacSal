namespace FacsalData.Migrations
{
    using SalaryEntities.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<FacsalData.FacsalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FacsalData.FacsalDbContext context)
        {
            var units = new List<Unit>
            {
                new Unit { Id = "ATH", Name = "ATHLETIC DEPARTMENT", Sequence = "T" },
                new Unit { Id = "S01", Name = "COLLEGE OF AGRICULTURE & LIFE SCIENCES", Sequence = "A"},
                new Unit { Id = "S02", Name = "COLLEGE OF ARCHITECTURE & URBAN STUDIES", Sequence = "B"},
                new Unit { Id = "S03", Name = "COLLEGE OF BUSINESS", Sequence = "C"},
            };
            units.ForEach(u => context.Units.AddOrUpdate(x => x.Id, u));
            context.SaveChanges();

            var depts = new List<Department>
            {
                new Department { Id = "0825", Name = "ATHLETIC DEPARTMENT", Sequence = "A", UnitID = "ATH"},
                new Department { Id = "0001", Name = "AGRICULTURAL AND APPLIED ECONOMICS", Sequence = "B", UnitID = "S01"},
                new Department { Id = "0002", Name = "BIOLOGICAL SYSTEMS ENGINEERING", Sequence = "B", UnitID = "S01"},
                new Department { Id = "0059", Name = "SCHOOL OF VISUAL ARTS", Sequence = "B", UnitID = "S02"},
                new Department { Id = "0151", Name = "SCHOOL OF ARCHITECTURE AND DESIGN", Sequence = "B", UnitID = "S02"},
                new Department { Id = "0070", Name = "ACCOUNTING AND INFORMATION SYSTEMS", Sequence = "B", UnitID = "S03"}
            };
            depts.ForEach(d => context.Departments.AddOrUpdate(x => x.Id, d));
            context.SaveChanges();
        }
    }
}
