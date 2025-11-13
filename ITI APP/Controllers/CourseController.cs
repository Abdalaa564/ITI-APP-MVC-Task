
namespace ITI_APP.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            var Courses = _courseService.GetAllCourses();
            return View(Courses);
        }

        [MessageResultFilter("hiiiiiiiiiiiii")]
        public IActionResult Details(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
                return NotFound();

            return View("detailsCourse", course);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = _courseService.GetCreateViewModel();
            return View("CreateCourse", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var viewModel = _courseService.GetEditViewModel(id);
            if (viewModel == null)
                return NotFound();

            return View("CreateCourse", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CourseFormViewModel model)
        {
            if (model.minDegree >= model.degree)
            {
                ModelState.AddModelError("minDegree", "Minimum degree cannot be greater than total degree");
            }

            if (!ModelState.IsValid)
            {
                model.DepartmentsSelectList = _courseService.GetCreateViewModel().DepartmentsSelectList;
                return View("CreateCourse", model);
            }

            try
            {
                if (model.Id == 0)
                {
                    _courseService.CreateCourse(model);
                }
                else
                {
                    bool updated = _courseService.UpdateCourse(model);
                    if (!updated) return NotFound();
                }

                TempData["SuccessMessage"] = model.Id == 0 ? "Course created successfully!" : "Course updated successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                model.DepartmentsSelectList = _courseService.GetCreateViewModel().DepartmentsSelectList;
                return View("CreateCourse", model);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _courseService.DeleteCourse(id);

            TempData["SuccessMessage"] = "Course deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
