
namespace ITI_APP.Models
{
    public class UniqueNameAttribute : ValidationAttribute
    {
        public string msg { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string newName = value.ToString()!.Trim();

            var context = (ITIEntities)validationContext.GetService(typeof(ITIEntities));
            if (context == null)
                throw new InvalidOperationException("DbContext not found in ValidationContext. Ensure it’s registered in the DI container.");

            var currentCourse = validationContext.ObjectInstance as Course;
            int currentId = currentCourse?.Id ?? 0;

            bool exists = context.Courses.Any(c => c.Name == newName && c.Id != currentId);

            if (exists)
                return new ValidationResult(msg ?? "Course name must be unique.");

            return ValidationResult.Success;
        }
    }
}