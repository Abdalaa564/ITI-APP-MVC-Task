
using ITI_APP.Services;

namespace ITI_APP.Controllers
{
    public class DeptController : Controller
    {
        private readonly ITIEntities _context;
        private readonly IDepartmentService _departmentService;

        public DeptController(ITIEntities context, IDepartmentService departmentService)
        {
            _context = context;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var departments = _departmentService.GetAllDepartments();
            return View("IndexDepartments", departments);
        }

        public IActionResult Details()
        {

            return View("DepartmentDetails");
        }
    }
}
