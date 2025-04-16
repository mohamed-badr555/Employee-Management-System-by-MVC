using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="FName is required")]
        public string FName { get; set; }
        [Required(ErrorMessage ="LName is required")]
        public string LName { get; set; }
        [Required(ErrorMessage= "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [MinLength(5, ErrorMessage = "password must be at least 5 characters")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; }
        public bool ISAgree { get; set; }

    }
}
