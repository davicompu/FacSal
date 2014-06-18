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
                new Unit { Id = "ATH", Name = "ATHLETIC DEPARTMENT", ValSeq = "T" },
                new Unit { Id = "S01", Name = "COLLEGE OF AGRICULTURE & LIFE SCIENCES", ValSeq = "A"},
                new Unit { Id = "S02", Name = "COLLEGE OF ARCHITECTURE & URBAN STUDIES", ValSeq = "B"},
                new Unit { Id = "S03", Name = "COLLEGE OF BUSINESS", ValSeq = "C"},
            };
            units.ForEach(u => context.Units.AddOrUpdate(x => x.Id, u));
            context.SaveChanges();

            var depts = new List<Department>
            {
                new Department { Id = "0825", Name = "ATHLETIC DEPARTMENT", ValSeq = "A", UnitId = "ATH"},
                new Department { Id = "0001", Name = "AGRICULTURAL AND APPLIED ECONOMICS", ValSeq = "B", UnitId = "S01"},
                new Department { Id = "0002", Name = "BIOLOGICAL SYSTEMS ENGINEERING", ValSeq = "B", UnitId = "S01"},
                new Department { Id = "0059", Name = "SCHOOL OF VISUAL ARTS", ValSeq = "B", UnitId = "S02"},
                new Department { Id = "0151", Name = "SCHOOL OF ARCHITECTURE AND DESIGN", ValSeq = "B", UnitId = "S02"},
                new Department { Id = "0070", Name = "ACCOUNTING AND INFORMATION SYSTEMS", ValSeq = "B", UnitId = "S03"}
            };
            depts.ForEach(d => context.Departments.AddOrUpdate(x => x.Id, d));
            context.SaveChanges();

            var appointmentTypes = new List<AppointmentType>
            {
                new AppointmentType { Id = 1, Name = "Full-time" }
            };
            appointmentTypes.ForEach(a => context.AppointmentTypes.AddOrUpdate(x => x.Name, a));
            context.SaveChanges();

            var facultyTypes = new List<FacultyType>
            {
                new FacultyType { Id = 1, Name = "Administrative Professional"},
                new FacultyType { Id = 2, Name = "Research Faculty" }
            };
            facultyTypes.ForEach(f => context.FacultyTypes.AddOrUpdate(x => x.Name, f));
            context.SaveChanges();

            var meritAdjustmentTypes = new List<MeritAdjustmentType>
            {
                new MeritAdjustmentType { Id = 1, Name = "Good performance" }
            };
            meritAdjustmentTypes.ForEach(m => context.MeritAdjustmentTypes.AddOrUpdate(x => x.Name, m));
            context.SaveChanges();

            var rankTypes = new List<RankType>
            {
                new RankType { Id = 1, Name = "Lecturer", ValSeq = "A"}
            };
            rankTypes.ForEach(r => context.RankTypes.AddOrUpdate(x => x.Name, r));
            context.SaveChanges();

            var specialAdjustmentTypes = new List<SpecialAdjustmentType>
            {
                new SpecialAdjustmentType { Id = 1, Name = "External market factors"},
                new SpecialAdjustmentType { Id = 2, Name = "Competitiveness"}
            };
            specialAdjustmentTypes.ForEach(s => context.SpecialAdjustmentTypes.AddOrUpdate(x => x.Name, s));
            context.SaveChanges();

            var statusTypes = new List<SalaryEntities.Entities.StatusType>
            {
                new SalaryEntities.Entities.StatusType { Id = 1, Name = "Active" },
                new SalaryEntities.Entities.StatusType { Id = 2, Name = "Inactive" }
            };
            statusTypes.ForEach(s => context.StatusTypes.AddOrUpdate(x => x.Name, s));
            context.SaveChanges();

            var persons = new List<Person>
            {
                new Person { Id = "001", Pid = "acampb", LastName = "Campbell", FirstName = "Allen", StatusTypeId = 2 },
                new Person { Id = "002", Pid = "mjordan", LastName = "Jordan", FirstName = "Michael", StatusTypeId = 1 }
            };
            persons.ForEach(p => context.Persons.AddOrUpdate(x => x.Pid, p));
            context.SaveChanges();

            var salaries = new List<Salary>
            {
                new Salary 
                { 
                    PersonId = "001", CycleYear = 2014, Title = "Director", FacultyTypeId = 2,
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

            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "update-0825" },
                new Role { Name = "update-0001" },
                new Role { Id = 3, Name = "read-0825" }
            };
            roles.ForEach(r => context.Roles.AddOrUpdate(x => x.Name, r));
            context.SaveChanges();

            var users = new List<User>
            {
                new User { Id = 1, Pid = "csherman",
                    RoleAssignments = new List<RoleAssignment> 
                    { 
                        new RoleAssignment { UserId = 1, RoleId = 1 },
                        new RoleAssignment { UserId = 1, RoleId = 1 }
                    } 
                }
            };
            users.ForEach(u => context.Users.AddOrUpdate(x => x.Pid, u));
            context.SaveChanges();
        }
    }
}
