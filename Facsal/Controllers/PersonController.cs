using SalaryEntities.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace Facsal.Controllers
{
    public class PersonController : ApiController
    {
        IUnitOfWork UnitOfWork;

        public PersonController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public IHttpActionResult GetDepartmentNames([FromUri]string id)
        {
            if (User.IsInRole("manage-all"))
            {
                return Ok(UnitOfWork.EmploymentRepository
                    .Find(e => e.PersonId == id)
                    .Select(e => e.Department.Name));
            }

            var userRoles = Roles.GetRolesForUser();

            var names = UnitOfWork.EmploymentRepository
                .Find(e => e.PersonId == id &&
                    userRoles.Contains("read-" + e.DepartmentId))
                .Select(e => e.Department.Name);
            
            return Ok(names);
        }
    }
}
