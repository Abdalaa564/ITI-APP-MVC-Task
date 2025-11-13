

namespace ITI_APP.ViewModels
{
    public class InstructorFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Instructor name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile ImageFile { get; set; }
        public string? Image { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com)$",
            ErrorMessage = "Email must be from Gmail")]
        [UniqueValue<Instructor>("Email", ErrorMessage = "Email already exists")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(3000, 50000, ErrorMessage = "Salary must be between 3,000 and 50,000")]
        [Column(TypeName = "decimal(18,4)")]
        [SalaryRange("DeptId")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Degree is required")]
        [Range(0, 100, ErrorMessage = "Degree must be between 0 and 100")]
        public int degree { get; set; }

        [Required(ErrorMessage = "Please select a department")]
        [Display(Name = "Department")]
        public int DeptId { get; set; }

        [Required(ErrorMessage = "Please select a course")]
        [Display(Name = "Course")]
        public int CrsId { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Departments { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Courses { get; set; }
    }
}
