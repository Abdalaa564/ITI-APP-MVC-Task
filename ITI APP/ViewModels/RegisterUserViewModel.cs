
namespace ITI_APP.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Please select a role")]
        public string Role { get; set; }
    }
}
