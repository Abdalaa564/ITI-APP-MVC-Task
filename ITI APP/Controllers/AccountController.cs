
namespace ITI_APP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);


            ApplicationUser user = new ApplicationUser();
            user.UserName = registerVM.UserName;
            user.Address = registerVM.Address;
            // create cookie
            //await _signInManager.SignInAsync(user, false);

            IdentityResult res = await _userManager.CreateAsync(user, registerVM.Password);
            if (res.Succeeded)
            {
                return RedirectToAction("Index", "Student");
            }
            else
            {
                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

            }
            return View(registerVM);
        }
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return RedirectToAction("Login");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null)
                return RedirectToAction("Login");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Address = "From Google Login",
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {

                    return RedirectToAction("Login");
                }
            }


            await _signInManager.SignInAsync(user, isPersistent: false);


            return RedirectToAction("Index", "Student");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel signInVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(signInVM.UserName);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, signInVM.PAssword);
                    await _signInManager.SignInAsync(user, signInVM.RememberMe);


                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register", "Account");
        }


    }
}

