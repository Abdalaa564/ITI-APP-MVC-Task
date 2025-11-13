namespace ITI_APP.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _uow;

        public CourseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _uow.Repository<Course>().GetAll(c => c.Departments);
        }

        //public IEnumerable<Course> GetAllCourses()
        //{
        //    IQueryable<Course> query = _context.Courses.AsNoTracking();
        //    Query = query.Include(c => c.Departments);
        //    return Query.ToList();
        //}

        public Course GetCourseById(int id)
        {
            var q = _uow.Repository<Course>()
                .GetAll(c => c.Departments, c => c.CrsResults, c => c.Instructors).AsQueryable();
            return q
                .Include(c => c.CrsResults).ThenInclude(cr => cr.Student)
                .FirstOrDefault(c => c.Id == id);
        }

        public CourseFormViewModel GetCreateViewModel()
        {
            var departments = _uow.Repository<Department>().GetAll();
            var viewModel = new CourseFormViewModel
            {
                DepartmentsSelectList = departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    })
                    .ToList()
            };
            return viewModel;
        }

        public CourseFormViewModel? GetEditViewModel(int id)
        {
            var course = _uow.Repository<Course>()
                .Find(c => c.Id == id, c => c.Departments).FirstOrDefault();

            if (course == null)
                return null;

            var departments = _uow.Repository<Department>().GetAll();
            var viewModel = new CourseFormViewModel
            {
                Id = course.Id,
                Name = course.Name,
                topic = course.topic,
                degree = course.degree,
                minDegree = course.minDegree,
                SelectedDepartments = course.Departments.Select(d => d.Id).ToList(),
                DepartmentsSelectList = departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    })
                    .ToList()
            };
            return viewModel;
        }

        public void CreateCourse(CourseFormViewModel model)
        {
            var course = new Course
            {
                Name = model.Name,
                topic = model.topic,
                degree = model.degree,
                minDegree = model.minDegree
            };

            course.Departments = _uow.Repository<Department>()
                .Find(d => model.SelectedDepartments.Contains(d.Id))
                .ToList();

            _uow.Repository<Course>().Add(course);
            _uow.Complete();
        }

        public bool UpdateCourse(CourseFormViewModel model)
        {
            var courseInDb = _uow.Repository<Course>()
                                 .Find(c => c.Id == model.Id, c => c.Departments).FirstOrDefault();

            if (courseInDb == null)
                return false;

            courseInDb.Name = model.Name;
            courseInDb.topic = model.topic;
            courseInDb.degree = model.degree;
            courseInDb.minDegree = model.minDegree;

            courseInDb.Departments.Clear();
            courseInDb.Departments = _uow.Repository<Department>()
                .Find(d => model.SelectedDepartments.Contains(d.Id))
                .ToList();

            _uow.Complete();
            return true;
        }

        public void DeleteCourse(int id)
        {
            var course = _uow.Repository<Course>().GetById(id);
            if (course == null) throw new Exception("Course not found");
            _uow.Repository<Course>().Remove(course);
            _uow.Complete();
        }
    }
}
