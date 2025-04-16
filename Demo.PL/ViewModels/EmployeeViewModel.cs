using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name can not be more than 50 characters")]
        [MinLength(5, ErrorMessage = "Name can not be less than 5 characters")]
        public string Name { get; set; }
        [Range(22, 30)]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Display(Name = "Hiring Date")]
        public DateTime HireDate { get; set; }
      public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        public int? DepartmentId { get; set; }

        public Department Department { get; set; }
    }
}
