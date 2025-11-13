namespace ITI_APP.Services
{
    public interface IInstructorService
    {
        IEnumerable<Instructor> GetAllInstructors();
        Instructor? GetInstructorById(int id);
        InstructorFormViewModel GetCreateViewModel();
        InstructorFormViewModel? GetEditViewModel(int id);
        void CreateInstructor(InstructorFormViewModel model, string? imagePath = null);
        bool UpdateInstructor(InstructorFormViewModel model, string? imagePath = null);
        bool DeleteInstructor(int id);
        bool InstructorExists(int id);
    }
}
