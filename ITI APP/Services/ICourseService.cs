namespace ITI_APP.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int id);
        CourseFormViewModel GetCreateViewModel();
        CourseFormViewModel? GetEditViewModel(int id);
        void CreateCourse(CourseFormViewModel model);
        bool UpdateCourse(CourseFormViewModel model);
        void DeleteCourse(int id);
    }
}
