namespace ITI_APP.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _uow;

        public DepartmentService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IEnumerable<Department> GetAllDepartments()
        {
            return _uow.Repository<Department>().GetAll().ToList();
        }
    }
}
