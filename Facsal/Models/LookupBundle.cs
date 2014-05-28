using SalaryEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facsal.Models
{
    public class LookupBundle
    {
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Department> Departments { get; set; }
    }
}