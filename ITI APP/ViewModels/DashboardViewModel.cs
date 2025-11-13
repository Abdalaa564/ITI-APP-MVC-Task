
namespace ITI_APP.ViewModels
{
    public class DashboardViewModel
    {
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Department> Departments { get; set; } = new List<Department>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<CrsResults> CrsResults { get; set; } = new List<CrsResults>();
        public List<CourseWithColor> CoursesWithColor { get; set; } = new List<CourseWithColor>();

    }
    public class CourseWithColor
    {
        public Course Course { get; set; }
        public string Color { get; set; }
    }
}
