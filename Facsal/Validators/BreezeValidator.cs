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
using ChrisJSherm.Extensions;

namespace Facsal.Validators
{
    public class BreezeValidator : IBreezeValidator
    {
        FacsalDbContext DbContext = new FacsalDbContext();

        public BreezeValidator() { }

        public bool BeforeSaveEntity(Breeze.ContextProvider.EntityInfo entityInfo)
        {
            if (entityInfo.EntityState == EntityState.Added)
            {
                if (entityInfo.Entity.GetType().IsSubclassOf(typeof(AuditEntityBase)))
                {
                    SetAuditEntityFields(entityInfo);
                }
            }
            
            if (entityInfo.Entity.GetType() == typeof(BaseSalaryAdjustment))
            {
                if (!IsAuthorizedToUpdateBaseSalaryAdjustment((BaseSalaryAdjustment)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify base salary adjustment entities.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            } 
            else if (entityInfo.Entity.GetType() == typeof(Employment))
            {
                if (!IsAuthorizedToUpdateEmployment((Employment)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify base employment entities.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            }
            else if (entityInfo.Entity.GetType() == typeof(Person))
            {
                if (!IsAuthorizedToUpdatePerson((Person)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify person entities.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            }
            else if (entityInfo.Entity.GetType() == typeof(RoleAssignment))
            {
                if (!IsAuthorizedToUpdateRoleAssignment((RoleAssignment)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify role assignment entities.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            }
            else if (entityInfo.Entity.GetType() == typeof(Salary))
            {
                if (!IsAuthorizedToUpdateSalary((Salary)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify this salary.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            }
            else if (entityInfo.Entity.GetType() == typeof(SpecialSalaryAdjustment))
            {
                if (!IsAuthorizedToUpdateSpecialSalaryAdjustment((SpecialSalaryAdjustment)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify this salary.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            }
            else if (entityInfo.Entity.GetType() == typeof(User))
            {
                if (!IsAuthorizedToUpdateUser((User)entityInfo.Entity))
                {
                    ThrowEntityError("You are not authorized to modify this user.",
                        "Unauthorized", HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                ThrowEntityError("You cannot use this method to modify this entity.",
                    "Bad request", HttpStatusCode.BadRequest);
            }

            //TypeSwitch.Do(
            //    entityInfo.Entity.GetType(),

            //    TypeSwitch.Case<BaseSalaryAdjustment>(x => 
            //        IsAuthorizedToUpdateBaseSalaryAdjustment(x)),

            //    TypeSwitch.Case<Employment>(x => 
            //        IsAuthorizedToUpdateEmployment(x)),

            //    TypeSwitch.Case<Person>(x => IsAuthorizedToUpdatePerson(x)),

            //    TypeSwitch.Case<RoleAssignment>(x => IsAuthorizedToUpdateRoleAssignment(x)),
                
            //    TypeSwitch.Case<Salary>(x => IsAuthorizedToUpdateSalary(x)),

            //    TypeSwitch.Case<SpecialSalaryAdjustment>(x => 
            //        IsAuthorizedToUpdateSpecialSalaryAdjustment(x)),

            //    TypeSwitch.Case<User>(x => IsAuthorizedToUpdateUser(x)));

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

        private bool IsAuthorizedToUpdateBaseSalaryAdjustment(BaseSalaryAdjustment adjustment)
        {
            if (HttpContext.Current.User.IsInRole("update-basesalaries"))
            {
                return true;
            }

            return false;
        }

        private bool IsAuthorizedToUpdateEmployment(Employment employment)
        {
            if (HttpContext.Current.User.IsInRole("update-employments"))
            {
                return true;
            }

            return false;
        }

        private bool IsAuthorizedToUpdatePerson(Person person)
        {
            if (HttpContext.Current.User.IsInRole("update-persons"))
            {
                return true;
            }

            return false;
        }

        private bool IsAuthorizedToUpdateRoleAssignment(RoleAssignment roleAssignment)
        {
            return false;
        }

        private bool IsAuthorizedToUpdateSalary(Salary salary)
        {
            var departments = DbContext.Employments
                .Where(e => e.PersonId == salary.PersonId)
                .Select(e => e.Department);

            foreach (var dept in departments)
            {
                if (HttpContext.Current.User.IsInRole("update-" + dept.Id))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAuthorizedToUpdateSpecialSalaryAdjustment(SpecialSalaryAdjustment adjustment)
        {
            var salary = DbContext.Salaries
                .FirstOrDefault(s => s.Id == adjustment.SalaryId);

            if (salary != null)
            {
                return IsAuthorizedToUpdateSalary(salary);
            }

            return false;
        }

        private bool IsAuthorizedToUpdateUser(User user)
        {
            return false;
        }

        private void ThrowEntityError(string errorMessage, string errorName, HttpStatusCode statusCode)
        {
            var errors = new List<EntityError>();
            errors.Add(new EntityError()
            {
                EntityTypeName = typeof(Salary).Name,
                ErrorMessage = errorMessage,
                ErrorName = errorName
            });
            var exception = new EntityErrorsException(errors);
            exception.StatusCode = statusCode;
            throw exception;
        }
    }
}