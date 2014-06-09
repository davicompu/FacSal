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
using System.Web.Http;

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
            return UnitOfWork.EmploymentRepository.All();
        }

        [HttpGet]
        public IQueryable<Person> Persons()
        {
            return UnitOfWork.PersonRepository.All();
        }

        // BreezeJS 1.4.12 does not support aggregate queries (count, sum, average, etc.).
        [HttpGet]
        public IQueryable<Person> PersonsWithMultipleEmployments()
        {
            return UnitOfWork.PersonRepository
                .Find(p => p.Employments.Count > 1);
        }

        [HttpGet]
        public IQueryable<Salary> Salaries()
        {
            return UnitOfWork.SalaryRepository.All();
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
                Departments = UnitOfWork.DepartmentRepository.All(),
                FacultyTypes = UnitOfWork.FacultyTypeRepository.All(),
                MeritAdjustmentTypes = UnitOfWork.MeritAdjustmentTypeRepository.All(),
                RankTypes = UnitOfWork.RankTypeRepository.All(),
                Roles = UnitOfWork.RoleRepository.All(),
                SpecialAdjustmentTypes = UnitOfWork.SpecialAdjustmentTypeRepository.All(),
                StatusTypes = UnitOfWork.StatusTypeRepository.All(),
                Units = UnitOfWork.UnitRepository.All()
            };
        }
    }
}
