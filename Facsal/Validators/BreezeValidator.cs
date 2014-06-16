using SalaryEntities.Entities;
using SalaryEntities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Breeze.ContextProvider;
using FacsalData;
using Breeze.ContextProvider.EF6;
using System.Net;

namespace Facsal.Validators
{
    public class BreezeValidator : IBreezeValidator
    {
        FacsalDbContext DbContext = new FacsalDbContext();

        public BreezeValidator() { }

        public bool BeforeSaveEntity(Breeze.ContextProvider.EntityInfo entityInfo)
        {
            if (entityInfo.Entity.GetType().IsSubclassOf(typeof(AuditEntityBase)))
            {
                if (entityInfo.EntityState == EntityState.Added)
                {
                    SetAuditEntityFields(entityInfo);
                }

                if (entityInfo.Entity.GetType() == typeof(Salary))
                {
                    var salary = (Salary)entityInfo.Entity;
                    var responsibleDepartments = DbContext.Employments
                        .Where(e => e.PersonId == salary.PersonId)
                        .Select(e => e.DepartmentId);

                    if (!UserIsInRoleCollection(responsibleDepartments))
                    {
                        var errors = new List<EntityError>();
                        errors.Add(new EntityError()
                            {
                                EntityTypeName = typeof(Salary).Name,
                                ErrorMessage = "Unauthorized to modify entities within this department.",
                                ErrorName = "Unauthorized"
                            });
                        var exception = new EntityErrorsException(errors);
                        exception.StatusCode = HttpStatusCode.Unauthorized;
                        throw exception;
                    }
                }
            }

            return true;
        }

        public Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            return saveMap;
        }
        private AuditEntityBase SetAuditEntityFields(Breeze.ContextProvider.EntityInfo entityInfo)
        {
            var entity = (AuditEntityBase)entityInfo.Entity;

            entity.CreatedBy = HttpContext.Current.User.Identity.Name;
            entity.CreatedDate = DateTime.UtcNow;

            return entity;
        }

        private bool UserIsInRoleCollection(IEnumerable<string> departmentIds)
        {
            foreach (var id in departmentIds)
            {
                if (HttpContext.Current.User.IsInRole("update-" + id))
                {
                    return true;
                }
            }

            return false;
        }
    }
}