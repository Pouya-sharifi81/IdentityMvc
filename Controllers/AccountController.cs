using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcBuggetoEx.Models;
using MvcBuggetoEx.Models.DTO;

namespace MvcBuggetoEx.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDto register)
        {
            if (ModelState.IsValid == false)
            {

                return View(register);

            }
            var newUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName=register.Email
            };
            var result =_userManager.CreateAsync(newUser , register.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index" , "Home");
            }

            var message = "";
            foreach(var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["message"] = message;
            return View(register);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginDto
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public IActionResult Login(LoginDto login)
        {
            if (ModelState.IsValid == false)
            {
                return View(login);
            }
            var user = _userManager.FindByNameAsync(login.UserName).Result;

            _signInManager.SignOutAsync();

            var result = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistens, true).Result;

            if (result.Succeeded)
            {
                return Redirect(login.ReturnUrl);
            }
            if (result.RequiresTwoFactor == true)
            {
                //
            }
            if (result.IsLockedOut)
            {
                //
            }
            ModelState.AddModelError(string.Empty, "Login Error");

            return View();
        }
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }
    }
}
