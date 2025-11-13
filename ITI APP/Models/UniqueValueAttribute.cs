
namespace ITI_APP.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueValueAttribute<TEntity> : ValidationAttribute where TEntity : class
    {
        private readonly string _propertyName;

        public UniqueValueAttribute(string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var context = (ITIEntities)validationContext.GetService(typeof(ITIEntities));
            if (context == null)
                return new ValidationResult("Database context not found.");

            var dbSet = context.Set<TEntity>();

            var idProperty = validationContext.ObjectType.GetProperty("Id");
            int currentId = 0;

            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(validationContext.ObjectInstance);
                if (idValue != null)
                    currentId = Convert.ToInt32(idValue);
            }

            bool exists = dbSet.Any(e =>
                EF.Property<string>(e, _propertyName).ToLower() == value.ToString().ToLower()
                && EF.Property<int>(e, "Id") != currentId);

            return exists
                ? new ValidationResult($"{_propertyName} must be unique.")
                : ValidationResult.Success;
        }
    }
}
