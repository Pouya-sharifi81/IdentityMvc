using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MvcBuggetoEx.Models;
using MvcBuggetoEx.Models.DTO;
using MvcBuggetoEx.Models.DTO.Account;
using MvcBuggetoEx.Service;

namespace MvcBuggetoEx.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly EmailService  _emailService;
        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = new EmailService();
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
                var token = _userManager.GenerateEmailConfirmationTokenAsync(newUser).Result;
                string callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    UserId = newUser.Id
              ,
                    token = token
                }, protocol: Request.Scheme);
                string body = $"لطفا برای فعال حساب کاربری بر روی لینک زیر کلیک کنید!  <br/> <a href={callbackUrl}> Link </a>";
                _emailService.Execute(newUser.Email, body, "فعال سازی حساب کاربری باگتو");

                return RedirectToAction("DisplayEmail");

            }


            var message = "";
            foreach(var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["message"] = message;
            return View(register);
        }
        public IActionResult ConfirmEmail(string UserId, string Token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return View("Error");
            }

            var result = _userManager.ConfirmEmailAsync(user, Token).Result;
            if (result.Succeeded)
            {
                /// return 
            }
            else
            {

            }
            return RedirectToAction("login");

        }
        public IActionResult DisplayEmail()
        {
            return View();
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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordConfirmationDto forgot)
        {
            if (!ModelState.IsValid)
            {

                return View(forgot);
            }
            var user = _userManager.FindByEmailAsync(forgot.Email).Result;
            if (user == null || _userManager.IsEmailConfirmedAsync(user).Result == false)
            {
                ViewBag.meesage = "ممکن است ایمیل وارد شده معتبر نباشد! و یا اینکه ایمیل خود را تایید نکرده باشید";
                return View();
            }
            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            string callbakUrl = Url.Action("ResetPassword", "Account", new
            {
                UserId = user.Id,
                token = token
            }, protocol: Request.Scheme);

            string body = $"برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید <br/> <a href={callbakUrl}> link reset Password </a>";
            _emailService.Execute(user.Email, body, "فراموشی رمز عبور");
            ViewBag.meesage = "لینک تنظیم مجدد کلمه عبور برای ایمیل شما ارسال شد";
            return View();
        }
        public IActionResult ResetPassword(string UserId, string Token)
        {
            return View(new ResetPasswordDto
            {
                Token = Token,
                UserId = UserId,
            });
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDto reset)
        {
            if (!ModelState.IsValid)
                return View(reset);
            if (reset.Password != reset.ConfirmPassword)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(reset.UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            var Result = _userManager.ResetPasswordAsync(user, reset.Token, reset.Password).Result;

            if (Result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            }
            else
            {
                ViewBag.Errors = Result.Errors;
                return View(reset);
            }

        }
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [Authorize]
        public IActionResult SetPhoneNumber()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SetPhoneNumber(SetPhoneNumberDto phoneNumberDro)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var setResult = _userManager.SetPhoneNumberAsync(user, phoneNumberDro.PhoneNumber).Result;
            string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumberDro.PhoneNumber).Result;
            SmsService smsService = new SmsService();
            smsService.Send(phoneNumberDro.PhoneNumber, code);
            TempData["PhoneNumber"] = phoneNumberDro.PhoneNumber;
            return RedirectToAction(nameof(VerifyPhoneNumber));
        }

        [Authorize]
        public IActionResult VerifyPhoneNumber()
        {

            return View(new VerifyPhoneNumberDto
            {
                PhoneNumber = TempData["PhoneNumber"].ToString(),
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult VerifyPhoneNumber(VerifyPhoneNumberDto verify)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            bool resultVerify = _userManager.VerifyChangePhoneNumberTokenAsync(user, verify.Code, verify.PhoneNumber).Result;
            if (resultVerify == false)
            {
                ViewData["Message"] = $"کد وارد شده برای شماره {verify.PhoneNumber} اشتباه اشت";
                return View(verify);
            }
            else
            {
                user.PhoneNumberConfirmed = true;
                _userManager.UpdateAsync(user);
            }
            return RedirectToAction("VerifySuccess");

        }


        public IActionResult VerifySuccess()
        {
            return View();
        }




    }
}
