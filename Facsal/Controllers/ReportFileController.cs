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
        IUnitOfWork UnitOfWork;
        ICollection<string> UserRoles;

        public ReportFileController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UserRoles = Roles.GetRolesForUser().ToList();
        }

        public ActionResult Meeting(string id)
        {
            var department = UnitOfWork.DepartmentRepository
                .FirstOrDefault(d => d.Id == id);

            var salaries = UnitOfWork.SalaryRepository
                .Find(s => department.Employees.Any(e => e.PersonId == s.PersonId));

            var report = new MeetingReport(department, salaries);

            return File(report.BinaryData, report.FileType, report.FileName);
        }
    }
}