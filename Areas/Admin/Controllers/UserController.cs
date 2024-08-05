using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcBuggetoEx.Areas.Admin.Models;
using MvcBuggetoEx.Models;
using MvcBuggetoEx.Models.DTO;

namespace MvcBuggetoEx.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users
                .Select(p => new UserListDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    UserName = p.UserName,
                    PhoneNumber = p.PhoneNumber,
                    EmailConfirmed = p.EmailConfirmed,
                    AccessFailedCount = p.AccessFailedCount
                }).ToList();
                  return View(users); 
                
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RegisterDto register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }

            User newUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.Email,
            };

            var result = _userManager.CreateAsync(newUser, register.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "user", new { area = "admin" });
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View(register);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            UserEditDto userEdit = new UserEditDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
            };
            return View(userEdit);

        }
        [HttpPost]
        public IActionResult Edit(UserEditDto userEdit)
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;
            user.FirstName = userEdit.FirstName;
            user.LastName = userEdit.LastName;
            user.PhoneNumber = userEdit.PhoneNumber;
            user.Email = userEdit.Email;
            user.UserName = userEdit.UserName;

            var result = _userManager.UpdateAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(userEdit);

        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            UserDeleteDto userDelete = new UserDeleteDto()
            {
                Email = user.Email,
                FullName = $"{user.FirstName}  {user.LastName}",
                Id = user.Id,
                UserName = user.UserName,
            };
            return View(userDelete);

        }
        [HttpPost]
        public IActionResult Delete(UserDeleteDto userDelete)
        {
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;
             var result =_userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View(userDelete);

        }

      

    }
}
