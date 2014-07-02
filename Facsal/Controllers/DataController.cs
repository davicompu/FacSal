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

        public DataController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public string Metadata()
        {
            return UnitOfWork.Metadata();
        }

        [HttpGet]
        public IQueryable<Employment> Employments()
        {
            var userRoles = Roles.GetRolesForUser();

            return UnitOfWork.EmploymentRepository
                .Find(e => userRoles
                .Contains("read-" + e.DepartmentId));
        }

        [HttpGet]
        public IQueryable<Salary> Salaries()
        {
            //if (User.IsInRole("read-all-entities"))
            //{
            //    return UnitOfWork.SalaryRepository.All();
            //}

            var userRoles = Roles.GetRolesForUser();

            return UnitOfWork.SalaryRepository
                .Find(s => s.Person.Employments
                    .Any(e => userRoles
                .Contains("read-" + e.DepartmentId)));
        }

        [HttpGet]
        public IQueryable<Salary> Salaries(string id)
        {
            if (User.IsInRole("read-" + id))
            {
                return UnitOfWork.SalaryRepository
                    .Find(s => s.Person.Employments.Any(e => e.DepartmentId == id))
                    .OrderBy(s => s.RankType.SequenceValue)
                        .ThenBy(s => s.Person.LastName);
            }

            return new List<Salary>().AsQueryable();
        }

        [HttpGet]
        public IQueryable<User> Users()
        {
            // TODO: User authorization.
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
            //if (User.IsInRole("read-all-entities"))
            //{
            //    return new LookupBundle
            //    {
            //        AppointmentTypes = UnitOfWork.AppointmentTypeRepository.All(),
            //        Departments = UnitOfWork.DepartmentRepository.All(),
            //        FacultyTypes = UnitOfWork.FacultyTypeRepository.All(),
            //        MeritAdjustmentTypes = UnitOfWork.MeritAdjustmentTypeRepository.All(),
            //        RankTypes = UnitOfWork.RankTypeRepository.All(),
            //        Roles = UnitOfWork.RoleRepository.All(),
            //        SpecialAdjustmentTypes = UnitOfWork.SpecialAdjustmentTypeRepository.All(),
            //        StatusTypes = UnitOfWork.StatusTypeRepository.All(),
            //        Units = UnitOfWork.UnitRepository.All(),
            //    };
            //}

            return new LookupBundle
            {
                AppointmentTypes = UnitOfWork.AppointmentTypeRepository.All(),
                Departments = UnitOfWork.RoleAssignmentRepository.Find(ra => ra.User.Pid == User.Identity.Name)
                    .Select(ra => ra.Role.Department),
                FacultyTypes = UnitOfWork.FacultyTypeRepository.All(),
                MeritAdjustmentTypes = UnitOfWork.MeritAdjustmentTypeRepository.All(),
                RankTypes = UnitOfWork.RankTypeRepository.All(),
                Roles = UnitOfWork.RoleAssignmentRepository.Find(ra => ra.User.Pid == User.Identity.Name)
                    .Select(ra => ra.Role),
                SpecialAdjustmentTypes = UnitOfWork.SpecialAdjustmentTypeRepository.All(),
                StatusTypes = UnitOfWork.StatusTypeRepository.All(),
                Units = UnitOfWork.RoleAssignmentRepository.Find(ra => ra.User.Pid == User.Identity.Name)
                    .Select(ra => ra.Role.Unit),
            };
        }
    }
}
