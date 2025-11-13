

namespace ITI_APP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> New(RoleViewModel newRoleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = newRoleVM.RoleName;
                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction(new RoleViewModel());
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(newRoleVM);
        }
    }
}
