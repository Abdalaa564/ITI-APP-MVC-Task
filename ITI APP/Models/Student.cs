
namespace ITI_APP.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(16, 35, ErrorMessage = "Age must be between 16 and 35")]
        public int age { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "GPA is required")]
        [Range(0.5, 4.0, ErrorMessage = "GPA must be between 0.5 and 4.0")]
        public double GPA { get; set; }

        [ForeignKey("Department")]
        [Required(ErrorMessage = "Please select a department")]
        public int DeptId { get; set; }
        public Department Department { get; set; }

        public virtual ICollection<CrsResults> CrsResults { get; set; } = new List<CrsResults>();
    }
}
