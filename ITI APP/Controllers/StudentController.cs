
namespace ITI_APP.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var Students = _studentService.GetAllStudents();
            return View(Students);
        }

        public IActionResult Details(int id)
        {
            var Student = _studentService.GetStudentById(id);

            if (Student == null)
                return NotFound();

            return View(Student);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Studentvm = _studentService.GetStudentFormData();

            return View(Studentvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(StudentFormViewModel model, IFormFile ImageFile, List<int> SelectedCourses)
        {
            try
            {

                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Address) || model.DeptId <= 0)
                {
                    ModelState.AddModelError("", "Please fill all required fields");
                    return SetupViewModelAndReturnView(model);
                }

                bool result = _studentService.SaveStudent(model, ImageFile, SelectedCourses);

                if (result)
                {
                    TempData["SuccessMessage"] = model.Id == 0 ? "Student created successfully!" : "Student updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Student not found or could not be saved.");
                    return SetupViewModelAndReturnView(model);
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return SetupViewModelAndReturnView(model);
            }
        }

        private IActionResult SetupViewModelAndReturnView(StudentFormViewModel model)
        {
            var viewModel = _studentService.GetStudentFormData();
            model.Departments = viewModel.Departments;
            model.Courses = viewModel.Courses;

            return View("Create", model);
        }

        [Authorize(Roles = "Admin, Instructor, Student")]
        public IActionResult Edit(int id)
        {
            var viewModel = _studentService.GetStudentForEdit(id);
            if (viewModel == null)
                return NotFound();

            return View("Create", viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool deleted = _studentService.DeleteStudent(id);

                if (!deleted)
                    TempData["ErrorMessage"] = "Student not found.";
                else
                    TempData["SuccessMessage"] = "Student deleted successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Grades(int id)
        {
            var viewModel = _studentService.GetStudentGrades(id);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, Instructor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGradeSingle([FromBody] UpdateGradeRequest request)
        {
            try
            {
                bool result = _studentService.UpdateStudentGrade(request.StudentName, request.CrsId, request.NewDegree);

                if (result)
                {
                    return Ok(new { success = true, message = "Grade updated successfully!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to update grade" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public class UpdateGradeRequest
        {
            public string StudentName { get; set; }
            public int CrsId { get; set; }
            public double NewDegree { get; set; }
        }

    }
}
