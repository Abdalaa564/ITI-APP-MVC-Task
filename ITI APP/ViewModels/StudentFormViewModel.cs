

namespace ITI_APP.ViewModels
{
    public class StudentFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(500, ErrorMessage = "Name must be between 3 and 50 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(16, 35, ErrorMessage = "Age must be between 16 and 35")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "GPA is required")]
        [Range(0.5, 4.0, ErrorMessage = "GPA must be between 0.5 and 4.0")]
        public double GPA { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Department")]
        public int DeptId { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        [Display(Name = "Select Courses")]
        public List<int> SelectedCourses { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> Courses { get; set; }
    }
}
