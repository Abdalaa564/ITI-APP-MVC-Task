

namespace ITI_APP.ViewModels
{
    public class CourseFormViewModel
    {
        public int Id { get; set; }

        //[UniqueValue<Course>("Name", ErrorMessage = "Course name must be unique.")]
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(40, ErrorMessage = "Name can't be longer than 40 characters")]
        [UniqueName(msg = "Course name must be unique.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Topic is required")]
        [StringLength(40, ErrorMessage = "Topic can't be longer than 40 characters")]
        public string topic { get; set; }

        [Required(ErrorMessage = "Degree is required")]
        [Range(60, 120, ErrorMessage = "Degree must be between 60 and 120")]
        public int degree { get; set; }

        [Required(ErrorMessage = "Minimum degree is required")]
        [Range(50, 60, ErrorMessage = "Min degree must be between 50 and 60")]
        public int minDegree { get; set; }

        [Required(ErrorMessage = "Please select at least one department")]
        public List<int> SelectedDepartments { get; set; } = new List<int>();

        [BindNever]
        public IEnumerable<SelectListItem>? DepartmentsSelectList { get; set; }
    }
}
