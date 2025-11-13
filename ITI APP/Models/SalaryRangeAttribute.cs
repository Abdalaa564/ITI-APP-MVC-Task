
namespace ITI_APP.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SalaryRangeAttribute : ValidationAttribute
    {
        private readonly string _departmentProperty;

        public SalaryRangeAttribute(string departmentProperty)
        {
            _departmentProperty = departmentProperty;
            ErrorMessage = "Invalid salary for selected department.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dbContext = (ITIEntities)validationContext.GetService(typeof(ITIEntities));
            if (dbContext == null)
                return new ValidationResult("Database context is not available.");

            var deptProperty = validationContext.ObjectType.GetProperty(_departmentProperty);
            if (deptProperty == null)
                return new ValidationResult($"Unknown property: {_departmentProperty}");

            var deptIdValue = deptProperty.GetValue(validationContext.ObjectInstance, null);
            if (deptIdValue == null)
                return ValidationResult.Success;

            if (!int.TryParse(deptIdValue.ToString(), out int deptId))
                return ValidationResult.Success;

            var department = dbContext.Departments.FirstOrDefault(d => d.Id == deptId);
            if (department == null)
                return ValidationResult.Success;

            var departmentName = department.Name;
            var salary = Convert.ToDecimal(value);

            if (departmentName.Equals("Software", StringComparison.OrdinalIgnoreCase))
            {
                if (salary <= 10000)
                    return new ValidationResult("For Software department, salary must be greater than 10000.");
            }
            else if (departmentName.Equals("Networks", StringComparison.OrdinalIgnoreCase))
            {
                if (salary < 15000 || salary > 50000)
                    return new ValidationResult("For Networks department, salary must be between 15000 and 50000.");
            }

            return ValidationResult.Success;
        }
    }
}
