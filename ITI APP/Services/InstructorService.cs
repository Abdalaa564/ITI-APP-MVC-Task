namespace ITI_APP.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _uow;

        public InstructorService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<Instructor> GetAllInstructors()
        {
            return _uow.Repository<Instructor>().GetAll(i => i.Course, i => i.Department);
        }

        public Instructor? GetInstructorById(int id)
        {
            return _uow.Repository<Instructor>()
                       .Find(i => i.Id == id, i => i.Course, i => i.Department).FirstOrDefault();
        }

        public InstructorFormViewModel GetCreateViewModel()
        {
            var departments = _uow.Repository<Department>().GetAll();
            var courses = _uow.Repository<Course>().GetAll();

            var viewModel = new InstructorFormViewModel
            {
                Departments = departments
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                    .ToList(),
                Courses = courses
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };

            return viewModel;
        }

        public InstructorFormViewModel? GetEditViewModel(int id)
        {
            var instructor = _uow.Repository<Instructor>().GetById(id);
            if (instructor == null)
                return null;

            var departments = _uow.Repository<Department>().GetAll();
            var courses = _uow.Repository<Course>().GetAll();
            var viewModel = new InstructorFormViewModel
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Salary = instructor.Salary,
                Email = instructor.Email,
                Address = instructor.Address,
                degree = instructor.degree,
                DeptId = instructor.DeptId,
                CrsId = instructor.CrsId,
                Image = instructor.Image,
                Departments = departments
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                    .ToList(),
                Courses = courses
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };
            return viewModel;
        }

        public void CreateInstructor(InstructorFormViewModel model, string? imagePath = null)
        {
            var instructor = new Instructor
            {
                Name = model.Name,
                Email = model.Email.ToLower(),
                Salary = model.Salary,
                Address = model.Address,
                degree = model.degree,
                DeptId = model.DeptId,
                CrsId = model.CrsId,
                Image = imagePath ?? "/images/instructors/default.png"
            };
            _uow.Repository<Instructor>().Add(instructor);
            _uow.Complete();
        }

        public bool UpdateInstructor(InstructorFormViewModel model, string? imagePath = null)
        {
            var instructor = _uow.Repository<Instructor>().GetById(model.Id);
            if (instructor == null)
                return false;

            instructor.Name = model.Name;
            //instructor.Email = model.Email.ToLower();
            instructor.Salary = model.Salary;
            instructor.Address = model.Address;
            instructor.degree = model.degree;
            instructor.DeptId = model.DeptId;
            instructor.CrsId = model.CrsId;
            if (imagePath != null)
                instructor.Image = imagePath;

            _uow.Repository<Instructor>().Update(instructor);
            _uow.Complete();
            return true;
        }

        public bool DeleteInstructor(int id)
        {
            var instructor = _uow.Repository<Instructor>().GetById(id);
            if (instructor == null)
                return false;
            _uow.Repository<Instructor>().Remove(instructor);
            _uow.Complete();
            return true;
        }

        public bool InstructorExists(int id)
        {
            return _uow.Repository<Instructor>().GetAll().Any(i => i.Id == id);
        }
    }
}
