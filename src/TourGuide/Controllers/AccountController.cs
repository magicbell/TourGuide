using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Models;
using System.Threading.Tasks;
using TourGuide.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TourGuide.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<TripUser> _signInManager;
        private UserManager<TripUser> _userManager;

        public AccountController(SignInManager<TripUser> signInManager, UserManager<TripUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Routes", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, true, false);
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(ReturnUrl))
                        return RedirectToAction("Routes", "Home");
                    return Redirect(ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                }
            }
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if(User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newUser = new TripUser { UserName = vm.UserName, Email = vm.Email };

                    IdentityResult idResult = await _userManager.CreateAsync(newUser, vm.Password);

                    if (idResult.Errors != null)
                    {
                        ModelState.AddModelError("", idResult.Errors.ToString());
                        return View();
                    }

                    return RedirectToAction("Routes", "Home");
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
            ModelState.AddModelError("", "Registration isn't complete");
            return View();
        }
    }
}
