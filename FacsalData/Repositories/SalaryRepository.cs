using SalaryEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacsalData.Repositories
{
    public class SalaryRepository : Repository<Salary>
    {
        protected DbContext Context { get; private set; }

        public SalaryRepository(DbContext context) : base(context)
        {
        }


    }
}
