using System.ComponentModel.DataAnnotations;

namespace NotesApp.Business
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter Your Email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}