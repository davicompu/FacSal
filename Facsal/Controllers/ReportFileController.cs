﻿using Facsal.Models.Files;
using FacsalData;
using SalaryEntities.Entities;
using SalaryEntities.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Facsal.Controllers
{
    public class ReportFileController : Controller
    {
        FacsalDbContext DbContext;
        ICollection<string> UserRoles;

        public ReportFileController(FacsalDbContext dbContext)
        {
            DbContext = dbContext;
            UserRoles = Roles.GetRolesForUser().ToList<string>();
        }

        public ActionResult DepartmentMeeting(string id)
        {
            if (User.IsInRole("manage-all") ||
                User.IsInRole("read-" + id))
            {
                var department = DbContext.Departments
                    .Where(d => d.Id == id)
                    .ToList()[0];

                var salaries = DbContext.Salaries
                    .Include("Person")
                    .Include("Person.Employments")
                    .Include("RankType")
                    .Include("AppointmentType")
                    .Where(s => s.Person.Employments.Any(e => e.DepartmentId == id) &&
                        s.Person.StatusTypeId == 1)
                    .OrderBy(s => s.RankType.SequenceValue)
                        .ThenBy(s => s.Person.LastName)
                    .ToList();

                var report = new MeetingReport(department, salaries);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }

        public ActionResult UnitMeeting(string id)
        {
            var departments = DbContext.Departments
                .Where(d => d.UnitId == id)
                .OrderBy(d => d.Name)
                .ToList();

            var authorizedDepartments = new List<Department>();

            foreach (var department in departments)
            {
                if (User.IsInRole("manage-all") ||
                    User.IsInRole("read-" + department.Id))
                {
                    authorizedDepartments.Add(department);
                }
            }

            if (authorizedDepartments.Count > 0)
            {
                var salaries = DbContext.Salaries
                    .Include("Person")
                    .Include("Person.Employments")
                    .Include("RankType")
                    .Include("AppointmentType")
                    .Where(s => s.Person.Employments.Any(e => e.Department.UnitId == id) &&
                        s.Person.StatusTypeId == 1)
                    .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName)
                    .ToList();

                var report = new MeetingReport(authorizedDepartments, salaries, true);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }

        public ActionResult DepartmentMeritSummary(string id)
        {
            if (User.IsInRole("manage-all") ||
                User.IsInRole("read-" + id))
            {
                var department = DbContext.Departments
                    .Where(d => d.Id == id)
                    .ToList()[0];

                var salaries = DbContext.Salaries
                    .Where(s => s.Person.Employments.Any(e => e.DepartmentId == id) &&
                        s.Person.StatusTypeId == 1)
                    .OrderBy(s => s.RankType.SequenceValue)
                        .ThenBy(s => s.Person.LastName)
                    .ToList();

                var report = new MeritSummaryReport(department, salaries);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }

        public ActionResult UnitMeritSummary(string id)
        {
            var departments = DbContext.Departments
                .Where(d => d.UnitId == id)
                .OrderBy(d => d.Name)
                .ToList();

            var authorizedDepartments = new List<Department>();

            foreach (var department in departments)
            {
                if (User.IsInRole("manage-all") ||
                    User.IsInRole("read-" + department.Id))
                {
                    authorizedDepartments.Add(department);
                }
            }

            if (authorizedDepartments.Count > 0)
            {
                var salaries = DbContext.Salaries
                    .Where(s => s.Person.Employments.Any(e => e.Department.UnitId == id) &&
                        s.Person.StatusTypeId == 1)
                    .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName)
                    .ToList();

                var report = new MeritSummaryReport(authorizedDepartments, salaries, true);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }



        public ActionResult DepartmentSalariesByFacultyType(string id)
        {
            if (User.IsInRole("manage-all") ||
                User.IsInRole("read-" + id))
            {
                var department = DbContext.Departments
                    .Where(d => d.Id == id)
                    .ToList()[0];

                var salaries = DbContext.Salaries
                    .Include("FacultyType")
                    .Where(s => s.Person.Employments.Any(e => e.DepartmentId == id) &&
                        s.Person.StatusTypeId == 1);


                var report = new SalariesByFacultyTypeReport(department, salaries);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }

        public ActionResult UnitSalariesByFacultyType(string id)
        {
            var departments = DbContext.Departments
                .Where(d => d.UnitId == id)
                .OrderBy(d => d.Name)
                .ToList();

            var authorizedDepartments = new List<Department>();

            foreach (var department in departments)
            {
                if (User.IsInRole("manage-all") ||
                    User.IsInRole("read-" + department.Id))
                {
                    authorizedDepartments.Add(department);
                }
            }

            if (authorizedDepartments.Count > 0)
            {
                var salaries = DbContext.Salaries
                    .Include("FacultyType")
                    .Where(s => s.Person.Employments.Any(e => e.Department.UnitId == id) &&
                        s.Person.StatusTypeId == 1);


                var report = new SalariesByFacultyTypeReport(authorizedDepartments, salaries, true);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }

        public ActionResult DepartmentMeetingAlternative(string id)
        {
            if (User.IsInRole("manage-all") ||
                User.IsInRole("read-" + id))
            {
                var department = DbContext.Departments
                    .Where(d => d.Id == id)
                    .ToList()[0];

                var salaries = DbContext.Salaries
                    .Include("Person")
                    .Include("Person.Employments")
                    .Include("RankType")
                    .Include("MeritAdjustmentType")
                    .Include("SpecialSalaryAdjustments")
                    .Include("SpecialSalaryAdjustments.SpecialAdjustmentType")
                    .Include("AppointmentType")
                    .Where(s => s.Person.Employments.Any(e => e.DepartmentId == id) &&
                        s.Person.StatusTypeId == 1)
                    .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName)
                    .ToList();

                var report = new MeetingAlternativeReport(department, salaries);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }

        public ActionResult UnitMeetingAlternative(string id)
        {
            var departments = DbContext.Departments
                .Where(d => d.UnitId == id)
                .OrderBy(d => d.Name)
                .ToList();

            var authorizedDepartments = new List<Department>();

            foreach (var department in departments)
            {
                if (User.IsInRole("manage-all") ||
                    User.IsInRole("read-" + department.Id))
                {
                    authorizedDepartments.Add(department);
                }
            }

            if (authorizedDepartments.Count > 0)
            {
                var salaries = DbContext.Salaries
                    .Include("Person")
                    .Include("Person.Employments")
                    .Include("RankType")
                    .Include("SpecialSalaryAdjustments")
                    .Include("SpecialSalaryAdjustments.SpecialAdjustmentType")
                    .Include("MeritAdjustmentType")
                    .Include("AppointmentType")
                    .Where(s => s.Person.Employments.Any(e => e.Department.UnitId == id) &&
                        s.Person.StatusTypeId == 1)
                    .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName)
                    .ToList();

                var report = new MeetingAlternativeReport(authorizedDepartments, salaries, false);

                return File(report.BinaryData, report.FileType, report.FileName);
            }

            return new HttpUnauthorizedResult();
        }


    }
}