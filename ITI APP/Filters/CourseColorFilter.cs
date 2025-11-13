
namespace ITI_APP.Filters
{
    public class CourseColorFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is DashboardController controller && context.Result is Microsoft.AspNetCore.Mvc.ViewResult viewResult)
            {
                var vm = viewResult.Model as DashboardViewModel;
                if (vm != null)
                {
                    vm.CoursesWithColor = vm.Courses.Select(c =>
                    {
                        if (c.Departments.Any(d => d.location == "Fayoum"))
                            return new CourseWithColor { Course = c, Color = "red" };

                        if (c.Departments.Any(d => d.location == "Smart"))
                            return new CourseWithColor { Course = c, Color = "yellow" };

                        return new CourseWithColor { Course = c, Color = "transparent" };
                    }).ToList();
                }
            }

            base.OnActionExecuted(context);
        }
    }
}
