using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public abstract class EntityBase
    {
        [ConcurrencyCheck]
        public int RowVersion { get; internal set; }
    }

    public abstract class AuditEntityBase : EntityBase
    {
        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedDate { get; set; }

        [Display(Name="Created By")]
        public string UserProfileId { get; set; }

        public ICollection<Modification> Modifications { get; set; }
    }

    public class Modification
    {
        public long ModificationId { get; set; }

        public string UpdatedBy {get;set;}

        [DataType(DataType.DateTime)]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
