namespace ITI_APP.ViewModels
{
    public class StudentGradesViewModel
    {
        public string studentName { get; set; }
        public List<CourseGrade> CourseGrades { get; set; }
        public double AverageGrade { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string Grade { get; set; }

    }
    public class CourseGrade
    {
        public int CrsId { get; set; }
        public string CourseName { get; set; }
        public double Degree { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; }

    }
}
