﻿using SalaryEntities.Entities;
using SalaryEntities.Repositories;
using SalaryEntities.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breeze.ContextProvider;
using System.Data.Entity.Validation;
using SalaryEntities.UnitOfWork;
using Breeze.ContextProvider.EF6;
using FacsalData.Repositories;

namespace FacsalData.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EFContextProvider<FacsalDbContext> ContextProvider;
        public IRepository<MeritAdjustmentType> MeritAdjustmentTypeRepository { get; private set; }
        public IRepository<AppointmentType> AppointmentTypeRepository { get; private set; }
        public IRepository<Department> DepartmentRepository { get; private set; }
        public IRepository<Employment> EmploymentRepository { get; private set; }
        public IRepository<FacultyType> FacultyTypeRepository { get; private set; }
        public IRepository<Person> PersonRepository { get; private set; }
        public IRepository<RankType> RankTypeRepository { get; private set; }
        public IRepository<Salary> SalaryRepository { get; private set; }
        public IRepository<Unit> UnitRepository { get; private set; }

        public UnitOfWork(IBreezeValidator breezeValidator)
        {
            ContextProvider = new EFContextProvider<FacsalDbContext>();
            ContextProvider.BeforeSaveEntitiesDelegate = breezeValidator.BeforeSaveEntities;
            ContextProvider.BeforeSaveEntityDelegate = breezeValidator.BeforeSaveEntity;

            MeritAdjustmentTypeRepository = new Repository<MeritAdjustmentType>(ContextProvider.Context);
            AppointmentTypeRepository = new Repository<AppointmentType>(ContextProvider.Context);
            DepartmentRepository = new Repository<Department>(ContextProvider.Context);
            EmploymentRepository = new Repository<Employment>(ContextProvider.Context);
            FacultyTypeRepository = new Repository<FacultyType>(ContextProvider.Context);
            PersonRepository = new Repository<Person>(ContextProvider.Context);
            RankTypeRepository = new Repository<RankType>(ContextProvider.Context);
            SalaryRepository = new Repository<Salary>(ContextProvider.Context);
            UnitRepository = new Repository<Unit>(ContextProvider.Context);
        }

        public int Commit()
        {
            try
            {
                return this.ContextProvider.Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public string Metadata()
        {
            return this.ContextProvider.Metadata();
        }

        public Breeze.ContextProvider.SaveResult Commit(Newtonsoft.Json.Linq.JObject changeSet)
        {
            try
            {
                return this.ContextProvider.SaveChanges(changeSet);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
