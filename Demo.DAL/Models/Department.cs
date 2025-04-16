using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department:ModelBase
    {
       
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name can not be more than 50 characters")]
        [MinLength(5, ErrorMessage = "Name can not be less than 5 characters")]
        public string Name { get; set; }
        [Display(Name = "Date of Creation")]
        public DateTime DateOfCreation { get; set; }


        //Navigation Properties 
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
