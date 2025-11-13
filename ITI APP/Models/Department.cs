namespace ITI_APP.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public string location { get; set; }

        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
