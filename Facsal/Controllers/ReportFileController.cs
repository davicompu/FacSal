using Facsal.Models.Files;
using FacsalData;
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
            var department = DbContext.Departments
                .Where(d => d.Id == id)
                .ToList()[0];

            var salaries = DbContext.Salaries
                .Include("Person")
                .Include("RankType")
                .Where(s => s.Person.Employments.Any(e => e.DepartmentId == id))
                .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName)
                .ToList();

            var report = new MeetingReport(department, salaries);

            return File(report.BinaryData, report.FileType, report.FileName);
        }

        public ActionResult UnitMeeting(string id)
        {
            var departments = DbContext.Departments
                .Where(d => d.UnitId == id)
                .ToList();

            var salaries = DbContext.Salaries
                .Include("Person")
                .Include("Person.Employments")
                .Include("RankType")
                .Where(s => s.Person.Employments.Any(e => e.Department.UnitId == id))
                .OrderBy(s => s.RankType.SequenceValue)
                    .ThenBy(s => s.Person.LastName)
                .ToList();

            var report = new MeetingReport(departments, salaries);

            return File(report.BinaryData, report.FileType, report.FileName);
        }
    }
}