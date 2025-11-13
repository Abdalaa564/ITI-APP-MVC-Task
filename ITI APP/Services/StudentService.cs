namespace ITI_APP.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _uow;

        public StudentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _uow.Repository<Student>().GetAll().ToList();
        }

        public Student? GetStudentById(int id)
        {
            var query = _uow.Repository<Student>()
                .GetAll(s => s.Department, s => s.CrsResults)
                .AsQueryable();

            return query
                .Include(s => s.CrsResults)
                .ThenInclude(cr => cr.Course)
                .FirstOrDefault(s => s.Id == id);
        }
        public StudentFormViewModel GetStudentFormData()
        {
            var departments = _uow.Repository<Department>().GetAll();
            var courses = _uow.Repository<Course>().GetAll();
            var viewModel = new StudentFormViewModel
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
        public StudentFormViewModel? GetStudentForEdit(int id)
        {
            var student = _uow.Repository<Student>()
                    .Find(s => s.Id == id, s => s.CrsResults).FirstOrDefault();
            if (student == null)
                return null;
            var departments = _uow.Repository<Department>().GetAll();
            var courses = _uow.Repository<Course>().GetAll();
            var viewModel = new StudentFormViewModel
            {
                Id = student.Id,
                Name = student.Name,
                Address = student.Address,
                Age = student.age,
                GPA = student.GPA,
                DeptId = student.DeptId,
                SelectedCourses = student.CrsResults.Select(cr => cr.CrsId).ToList(),
                Departments = departments
                    .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                    .ToList(),
                Courses = courses
                    .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                    .ToList()
            };
            return viewModel;
        }

        public bool SaveStudent(StudentFormViewModel model, IFormFile ImageFile, List<int> SelectedCourses)
        {
            //_uow.BeginTransaction();
            try
            {
                var crsResultRepo = _uow.Repository<CrsResults>();
                Student student;
                if (model.Id == 0)
                {
                    student = new Student();
                    _uow.Repository<Student>().Add(student);
                }
                else
                {
                    student = _uow.Repository<Student>()
                        .Find(s => s.Id == model.Id, s => s.CrsResults)
                        .FirstOrDefault()!;
                    if (student == null)
                    {
                        return false;
                    }
                }
                student.Name = model.Name;
                student.Address = model.Address;
                student.age = model.Age;
                student.GPA = model.GPA;
                student.DeptId = model.DeptId;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    student.Image = "/uploads/" + fileName;
                }
                else if (model.Id == 0)
                {
                    student.Image = "/uploads/default.png";
                }
                _uow.Complete();

                if (model.Id > 0)
                {
                    var existingCourses = crsResultRepo.Find(c => c.StudentId == student.Id).ToList();
                    if (existingCourses.Any())
                    {
                        foreach (var course in existingCourses)
                            crsResultRepo.Remove(course);
                    }
                }

                if (SelectedCourses != null && SelectedCourses.Any())
                {
                    foreach (var courseId in SelectedCourses)
                    {
                        var crsResult = new CrsResults
                        {
                            StudentId = student.Id,
                            CrsId = courseId,
                            Degree = 0
                        };
                        crsResultRepo.Add(crsResult);
                    }
                }

                _uow.Complete();
                //_uow.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "❌ Error occurred while saving student ID={Id}", model.Id);
                //_uow.RollbackTransaction();
                throw;
            }
        }
        public bool DeleteStudent(int id)
        {
            _uow.BeginTransaction();
            try
            {
                var student = _uow.Repository<Student>()
                    .Find(s => s.Id == id, s => s.CrsResults)
                    .FirstOrDefault();

                if (student == null)
                    return false;

                var crsResultRepo = _uow.Repository<CrsResults>();
                var existingCourses = crsResultRepo.Find(c => c.StudentId == student.Id).ToList();
                if (student.CrsResults != null && existingCourses.Any())
                {
                    foreach (var course in existingCourses)
                        crsResultRepo.Remove(course);
                }
                _uow.Repository<Student>().Remove(student);
                _uow.Complete();
                _uow.CommitTransaction();
                return true;
            }
            catch
            {
                _uow.RollbackTransaction();
                throw;
            }
        }

        public StudentGradesViewModel GetStudentGrades(int id)
        {
            var q = _uow.Repository<Student>()
                .GetAll(s => s.CrsResults).AsQueryable();
            var student = q
                .Include(s => s.CrsResults)
                    .ThenInclude(cr => cr.Course)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
                return null;

            foreach (var cr in student.CrsResults)
            {
                if (cr.Course == null)
                {
                    Log.Warning($"Course is null for CrsResultId = {cr.CrsId}");
                }
            }
            if (student.CrsResults == null || !student.CrsResults.Any())
            {
                return new StudentGradesViewModel
                {
                    studentName = student.Name,
                    CourseGrades = new List<CourseGrade>(),
                    AverageGrade = student.GPA,
                    Status = student.GPA >= 2.0 ? "Pass ✅" : "Fail ❌",
                    StatusColor = student.GPA >= 2.0 ? "green" : "red"
                };
            }

            var courseGrades = student.CrsResults.Select(cr => new CourseGrade
            {
                CrsId = cr.CrsId,
                CourseName = cr.Course.Name,
                Degree = cr.Degree,
                Grade = GetGrade(cr.Degree),
                Status = cr.Degree >= 60 ? "Pass ✅" : "Fail ❌"
            }).ToList();

            var averageGrade = courseGrades.Average(cg => cg.Degree);
            var status = averageGrade >= 60 ? "Pass ✅" : "Fail ❌";
            var statusColor = averageGrade >= 60 ? "green" : "red";

            var viewModel = new StudentGradesViewModel
            {
                studentName = student.Name,
                CourseGrades = courseGrades,
                AverageGrade = averageGrade,
                Status = status,
                StatusColor = statusColor
            };
            return viewModel;
        }

        private string GetGrade(double degree)
        {
            if (degree >= 90) return "A";
            if (degree >= 80) return "B";
            if (degree >= 70) return "C";
            if (degree >= 60) return "D";
            return "F";
        }

        public bool UpdateStudentGrade(string studentName, int crsId, double newDegree)
        {
            try
            {
                var student = _uow.Repository<Student>()
                    .Find(s => s.Name == studentName)
                    .FirstOrDefault();

                if (student == null)
                {
                    Log.Warning($"Student with name {studentName} not found");
                    return false;
                }

                var crsResult = _uow.Repository<CrsResults>()
                    .Find(cr => cr.StudentId == student.Id && cr.CrsId == crsId)
                    .FirstOrDefault();

                if (crsResult == null)
                {
                    return false;
                }

                crsResult.Degree = (int)newDegree;

                _uow.Complete();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error updating grade for student {studentName} in course {crsId}");
                return false;
            }
        }

    }
}
