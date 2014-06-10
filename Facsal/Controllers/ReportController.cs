using FacsalData;
using SalaryEntities.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Facsal.Controllers
{
    public class ReportController : ApiController
    {
        FacsalDbContext UnitOfWork;

        public ReportController(FacsalDbContext unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public IHttpActionResult GetSalariesByFacultyType([FromUri]int id)
        {
            var salaries = UnitOfWork.Salaries
                .Where(s => s.Person.Employments.Any(e => e.DepartmentId == "0825"))
                .GroupBy(s => s.FacultyTypeId)
                .Select(sg => new {
                    FacultyType = sg.Select(x => x.FacultyType.Name),
                    StartingSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                        x.EminentAmount + x.PromotionAmount),
                    MeritIncreases = sg.Sum(x => x.MeritIncrease),
                    SpecialIncreases = sg.Sum(x => x.SpecialIncrease),
                    EminentIncreases = sg.Sum(x => x.EminentIncrease),
                    NewSalaries = sg.Sum(x => x.BaseAmount + x.AdminAmount +
                        x.EminentAmount + x.PromotionAmount + x.MeritIncrease +
                        x.SpecialIncrease + x.EminentIncrease)
                });

            return Ok(salaries);
        }
    }
}
