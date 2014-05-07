using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryEntities.Entities
{
    public class Person : AuditEntityBase
    {
        [Key]
        public string Id { get; set; }

        public string Pid { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Salary> Salaries { get; set; }

        public virtual ICollection<Employment> Employments { get; set; }

        /* Full name not derived since EF couldn't use CONTAINS on derived 
         * properties at the time of development. CONTAINS needed for Search 
         * functionality --> DataFetcher.GetPersonnelSearchForCurrentUser
        */
        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
            set
            {
                string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string Email
        {
            get
            {
                return string.Format("{0}@vt.edu", Pid);
            }
        }
    }
}
