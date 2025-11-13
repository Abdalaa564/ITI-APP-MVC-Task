namespace ITI_APP.Services
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAllStudents();
        Student? GetStudentById(int id);
        StudentFormViewModel GetStudentFormData();
        StudentFormViewModel? GetStudentForEdit(int id);
        bool SaveStudent(StudentFormViewModel model, IFormFile ImageFile, List<int> SelectedCourses);
        bool DeleteStudent(int id);
        StudentGradesViewModel GetStudentGrades(int id);
        bool UpdateStudentGrade(string studentName, int crsId, double newDegree);
    }
}
