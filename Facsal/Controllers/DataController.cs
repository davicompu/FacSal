using Breeze.ContextProvider;
using Breeze.WebApi2;
using Facsal.Models;
using Newtonsoft.Json.Linq;
using SalaryEntities.Entities;
using SalaryEntities.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Facsal.Controllers
{
    [BreezeController]
    public class DataController : ApiController
    {
        IUnitOfWork UnitOfWork;
        ICollection<Role> UserRoles;
        ICollection<string> UserRoleNames;

        public DataController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UserRoles = UnitOfWork.RoleAssignmentRepository
                .Find(ra => ra.User.Pid == User.Identity.Name)
                .Select(ra => ra.Role)
                .ToList();

            UserRoleNames = UserRoles
                .Select(r => r.Name)
                .ToList();
        }

        [HttpGet]
        public string Metadata()
        {
            return UnitOfWork.Metadata();
        }

        [HttpGet]
        public IQueryable<BaseSalaryAdjustment> BaseSalaryAdjustments()
        {
            return UnitOfWork.BaseSalaryAdjustmentRepository.All();
        }

        [HttpGet]
        public IQueryable<Department> Departments()
        {
            return UnitOfWork.DepartmentRepository.All();
        }

        [HttpGet]
        public IQueryable<Employment> Employments()
        {
            return UnitOfWork.EmploymentRepository
                .Find(e => UserRoleNames
                    .Any(ur => ur == "read-" + e.DepartmentId));
        }

        [HttpGet]
        public IQueryable<Person> Persons()
        {
            return UnitOfWork.PersonRepository.All();
        }

        [HttpGet]
        public IQueryable<Salary> Salaries()
        {
            return UnitOfWork.SalaryRepository
                .Find(s => s.Person.Employments
                    .Any(e => UserRoleNames
                        .Any(ur => ur == "read-" + e.DepartmentId)));
        }

        [HttpGet]
        public IQueryable<Unit> Units()
        {
            return UnitOfWork.UnitRepository.All();
        }

        [HttpGet]
        public IQueryable<User> Users()
        {
            return UnitOfWork.UserRepository.All();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return UnitOfWork.Commit(saveBundle);
        }

        [HttpGet]
        public LookupBundle GetLookups()
        {
            return new LookupBundle
            {
                AppointmentTypes = UnitOfWork.AppointmentTypeRepository.All(),
                Departments = UnitOfWork.DepartmentRepository
                    .Find(d => UserRoleNames.Any(ur => ur == "read-" + d.Id)),
                FacultyTypes = UnitOfWork.FacultyTypeRepository.All(),
                MeritAdjustmentTypes = UnitOfWork.MeritAdjustmentTypeRepository.All(),
                RankTypes = UnitOfWork.RankTypeRepository.All(),
                Roles = UserRoles,
                SpecialAdjustmentTypes = UnitOfWork.SpecialAdjustmentTypeRepository.All(),
                StatusTypes = UnitOfWork.StatusTypeRepository.All(),
                Units = UnitOfWork.UnitRepository
                    .Find(u => u.Departments.Any(d => UserRoleNames.Any(ur => ur == "read-" + d.Id)))
            };
        }
    }
}
