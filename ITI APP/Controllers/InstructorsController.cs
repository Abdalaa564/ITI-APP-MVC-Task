
namespace ITI_APP.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IInstructorService _instructorService;

        public InstructorsController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [Authorize(Roles = "Admin, Instructor")]
        public IActionResult Index()
        {
            var instructors = _instructorService.GetAllInstructors();
            return View(instructors);
        }

        [Authorize(Roles = "Admin, Instructor")]
        public IActionResult Details(int id)
        {
            if (!_instructorService.InstructorExists(id))
                return NotFound();

            var instructor = _instructorService.GetInstructorById(id);
            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = _instructorService.GetCreateViewModel();

            return View("CreateInst", viewModel);
        }

        [Authorize(Roles = "Admin, Instructor")]
        public IActionResult Edit(int id)
        {
            if (!_instructorService.InstructorExists(id))
                return NotFound();

            var viewModel = _instructorService.GetEditViewModel(id);
            if (viewModel == null)
                return NotFound();

            return View("CreateInst", viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(InstructorFormViewModel model)
        {
            try
            {
                //Log.Warning($"DEBUG => Name: {model.Name}, Email: {model.Email}, DeptId: {model.DeptId}, CrsId: {model.CrsId}");
                //Log.Warning($"🟢 Save Action Started - ID: {model.Id}, Name: {model.Name}");
                ModelState.Remove("Courses");
                ModelState.Remove("Departments");

                if (!ModelState.IsValid)
                {
                    //Log.Warning($"Form Values => DeptId={model.DeptId}, CrsId={model.CrsId}");
                    //ModelState.Clear();
                    model.Departments = _instructorService.GetCreateViewModel().Departments;
                    model.Courses = _instructorService.GetCreateViewModel().Courses;
                    return View("CreateInst", model);
                }

                string? imagePath = null;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/instructors");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ImageFile.CopyTo(stream);
                    }

                    imagePath = $"images/instructors/{fileName}";
                }

                if (model.Id == 0)
                {
                    _instructorService.CreateInstructor(model, imagePath);
                }
                else
                {
                    bool updated = _instructorService.UpdateInstructor(model, imagePath);
                    if (!updated) return NotFound();
                }

                TempData["SuccessMessage"] = model.Id == 0 ? "Instructor created successfully!" : "Instructor updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                model.Departments = _instructorService.GetCreateViewModel().Departments;
                model.Courses = _instructorService.GetCreateViewModel().Courses;
                return View("CreateInst", model);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (!_instructorService.InstructorExists(id))
                return NotFound();

            var instructor = _instructorService.GetInstructorById(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = _instructorService.DeleteInstructor(id);
            if (!deleted)
                return NotFound();

            TempData["SuccessMessage"] = "Instructor deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
