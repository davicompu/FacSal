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
                new Department { Id = "0825", Name = "ATHLETIC DEPARTMENT", Sequence = "A", UnitId = "ATH"},
                new Department { Id = "0001", Name = "AGRICULTURAL AND APPLIED ECONOMICS", Sequence = "B", UnitId = "S01"},
                new Department { Id = "0002", Name = "BIOLOGICAL SYSTEMS ENGINEERING", Sequence = "B", UnitId = "S01"},
                new Department { Id = "0059", Name = "SCHOOL OF VISUAL ARTS", Sequence = "B", UnitId = "S02"},
                new Department { Id = "0151", Name = "SCHOOL OF ARCHITECTURE AND DESIGN", Sequence = "B", UnitId = "S02"},
                new Department { Id = "0070", Name = "ACCOUNTING AND INFORMATION SYSTEMS", Sequence = "B", UnitId = "S03"}
            };
            depts.ForEach(d => context.Departments.AddOrUpdate(x => x.Id, d));
            context.SaveChanges();

            var facultyTypes = new List<FacultyType>
            {
                new FacultyType { Id = 1, Name = "Administrative Professional"}
            };
            facultyTypes.ForEach(f => context.FacultyTypes.AddOrUpdate(x => x.Name, f));
            context.SaveChanges();

            var rankTypes = new List<RankType>
            {
                new RankType { Id = 1, Name = "Lecturer"}
            };
            rankTypes.ForEach(r => context.RankTypes.AddOrUpdate(x => x.Name, r));
            context.SaveChanges();

            var appointmentTypes = new List<AppointmentType>
            {
                new AppointmentType { Id = 1, Name = "Full-time" }
            };
            appointmentTypes.ForEach(a => context.AppointmentTypes.AddOrUpdate(x => x.Name, a));
            context.SaveChanges();

            var meritAdjustmentTypes = new List<MeritAdjustmentType>
            {
                new MeritAdjustmentType { Id = 1, Name = "Good performance" }
            };
            meritAdjustmentTypes.ForEach(m => context.MeritAdjustmentTypes.AddOrUpdate(x => x.Name, m));
            context.SaveChanges();

            var persons = new List<Person>
            {
                new Person { Id = "001", Pid = "acampb", LastName = "Campbell", FirstName = "Allen", IsActive = true },
                new Person { Id = "002", Pid = "mjordan", LastName = "Jordan", FirstName = "Michael", IsActive = true }
            };
            persons.ForEach(p => context.Persons.AddOrUpdate(x => x.Pid, p));
            context.SaveChanges();

            var salaries = new List<Salary>
            {
                new Salary 
                { 
                    PersonId = "001", CycleYear = 2014, Title = "Director", FacultyTypeId = 1,
                    FullTimeEquivalent = 1.00M, BaseAmount = 1000, AdminAmount = 0, EminentAmount = 0,
                    PromotionAmount = 0, RankTypeId = 1, AppointmentTypeId = 1, MeritAdjustmentTypeId = 1
                },
                new Salary
                {
                    PersonId = "002", CycleYear = 2014, Title = "Director", FacultyTypeId = 1,
                    FullTimeEquivalent = 1.00M, BaseAmount = 2000, AdminAmount = 0, EminentAmount = 0,
                    PromotionAmount = 0, RankTypeId = 1, AppointmentTypeId = 1, MeritAdjustmentTypeId = 1
                }
            };
            salaries.ForEach(s => context.Salaries.AddOrUpdate(x => x.PersonId, s));
            context.SaveChanges();

            var employments = new List<Employment>
            {
                new Employment { PersonId = "001", DepartmentId = "0825" },
                new Employment { PersonId = "002", DepartmentId = "0825" }
            };
            employments.ForEach(e => context.Employments.AddOrUpdate(x => x.PersonId, e));
            context.SaveChanges();
        }
    }
}
