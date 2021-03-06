﻿using Breeze.ContextProvider;
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
        public IQueryable<Salary> BaseSalaryAdjustments()
        {
            if (User.IsInRole("manage-all"))
            {
                return UnitOfWork.SalaryRepository
                    .Find(s => s.BannerBaseAmount != s.BaseAmount);
            }

            var userRoles = Roles.GetRolesForUser();

            return UnitOfWork.SalaryRepository
                .Find(s => s.Person.Employments
                    .Any(e => userRoles
                        .Contains("read-" + e.DepartmentId)) &&
                    s.BannerBaseAmount != s.BaseAmount)
                .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName);
        }

        [HttpGet]
        public IQueryable<Employment> Employments()
        {
            if (User.IsInRole("manage-all"))
            {
                return UnitOfWork.EmploymentRepository.All();
            }

            var userRoles = Roles.GetRolesForUser();

            return UnitOfWork.EmploymentRepository
                .Find(e => userRoles
                .Contains("read-" + e.DepartmentId));
        }

        [HttpGet]
        public IQueryable<Role> GetRoles()
        {
            return UnitOfWork.RoleRepository.All();
        }

        [HttpGet]
        public IQueryable<Unit> GetManageableUnits()
        {
            return UnitOfWork.RoleAssignmentRepository
                .Find(ra => ra.User.Pid == User.Identity.Name &&
                    ra.Role.Name.StartsWith("manage-users-"))
                .Select(ra => ra.Role.Department.Unit);
        }

        [HttpGet]
        public IQueryable<Salary> Salaries()
        {
            if (User.IsInRole("manage-all"))
            {
                return UnitOfWork.SalaryRepository.All()
                    .OrderBy(s => s.RankType.SequenceValue)
                        .ThenBy(s => s.Person.LastName);
            }

            var userRoles = Roles.GetRolesForUser();

            return UnitOfWork.SalaryRepository
                .Find(s => s.Person.Employments
                    .Any(e => userRoles
                        .Contains("read-" + e.DepartmentId)))
                .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName);
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
            if (User.IsInRole("manage-all"))
            {
                return new LookupBundle
                {
                    AppointmentTypes = UnitOfWork.AppointmentTypeRepository.All(),
                    Departments = UnitOfWork.DepartmentRepository.All()
                        .OrderBy(d => d.Name),
                    FacultyTypes = UnitOfWork.FacultyTypeRepository.All(),
                    LeaveTypes = UnitOfWork.LeaveTypeRepository.All(),
                    MeritAdjustmentTypes = UnitOfWork.MeritAdjustmentTypeRepository.All(),
                    RankTypes = UnitOfWork.RankTypeRepository.All(),
                    SpecialAdjustmentTypes = UnitOfWork.SpecialAdjustmentTypeRepository.All(),
                    StatusTypes = UnitOfWork.StatusTypeRepository.All(),
                    Units = UnitOfWork.UnitRepository.All()
                            .OrderBy(u => u.Name),
                };
            }

            return new LookupBundle
            {
                AppointmentTypes = UnitOfWork.AppointmentTypeRepository.All(),
                Departments = UnitOfWork.RoleAssignmentRepository.Find(ra => ra.User.Pid == User.Identity.Name)
                    .Select(ra => ra.Role.Department)
                    .OrderBy(d => d.Name),
                FacultyTypes = UnitOfWork.FacultyTypeRepository.All(),
                LeaveTypes = UnitOfWork.LeaveTypeRepository.All(),
                MeritAdjustmentTypes = UnitOfWork.MeritAdjustmentTypeRepository.All(),
                RankTypes = UnitOfWork.RankTypeRepository.All(),
                SpecialAdjustmentTypes = UnitOfWork.SpecialAdjustmentTypeRepository.All(),
                StatusTypes = UnitOfWork.StatusTypeRepository.All(),
                Units = UnitOfWork.RoleAssignmentRepository.Find(ra => ra.User.Pid == User.Identity.Name)
                    .Select(ra => ra.Role.Department.Unit)
                    .OrderBy(u => u.Name),
            };
        }
    }
}
