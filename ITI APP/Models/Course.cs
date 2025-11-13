
namespace ITI_APP.Models
{
    public class Course
    {
        public int Id { get; set; }

        //[UniqueValue<Course>("Name", ErrorMessage = "Course name must be unique.")]
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(40, ErrorMessage = "Name can't be longer than 40 characters")]
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

        public  ICollection<CrsResults> CrsResults { get; set; } = new List<CrsResults>();

        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}
