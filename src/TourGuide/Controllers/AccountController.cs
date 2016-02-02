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

        public AccountController(SignInManager<TripUser> signInManager)
        {
            _signInManager = signInManager;
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
    }
}
