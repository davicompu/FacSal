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
        public IQueryable<Unit> Units()
        {
            return UnitOfWork.UnitRepository.All();
        }

        [HttpGet]
        public IQueryable<Department> Departments()
        {
            return UnitOfWork.DepartmentRepository.All();
        }

        [HttpGet]
        public IQueryable<Salary> Salaries()
        {
            return UnitOfWork.SalaryRepository.All();
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
                Units = UnitOfWork.UnitRepository.All(),
                Departments = UnitOfWork.DepartmentRepository.All()
            };
        }
    }
}
