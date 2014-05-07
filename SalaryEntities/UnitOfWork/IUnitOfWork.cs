using Breeze.ContextProvider;
using Newtonsoft.Json.Linq;
using SalaryEntities.Entities;
using SalaryEntities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<AdjustmentType> AdjustmentTypeRepository { get; }
        IRepository<AppointmentType> AppointmentTypeRepository { get; }
        IRepository<Department> DepartmentRepository { get; }
        IRepository<Employment> EmploymentRepository { get; }
        IRepository<FacultyType> FacultyTypeRepository { get; }
        IRepository<Person> PersonRepository { get; }
        IRepository<RankType> RankTypeRepository { get; }
        IRepository<Salary> SalaryRepository { get; }
        IRepository<Unit> UnitRepository { get; }

        int Commit();

        // Breeze specific.
        string Metadata();
        SaveResult Commit(JObject changeSet);
    }
}
