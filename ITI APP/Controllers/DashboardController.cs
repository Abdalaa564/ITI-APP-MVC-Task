
namespace ITI_APP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ITIEntities _context;

        public DashboardController(ITIEntities context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [CourseColorFilter]
        public IActionResult Dashboard()
        {
            var viewModel = new DashboardViewModel
            {
                Students = _context.Students.ToList(),
                Instructors = _context.Instructors.ToList(),
                Courses = _context.Courses.Include(c => c.Departments)
                    .Include(c => c.Instructors).ToList(),
                Departments = _context.Departments.ToList()
            };

            return View("HomeDashboard", viewModel);
        }
    }
}
