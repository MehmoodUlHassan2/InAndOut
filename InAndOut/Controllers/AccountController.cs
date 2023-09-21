using InAndOut.Utility;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InAndOut.Services;

namespace InAndOut.Controllers

{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMailService _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMailService emailSender)// IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }




        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registration(AuthViewModel model)
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = model.UserRegistrationModel.UserName,
                Email = model.UserRegistrationModel.Email,
                PhoneNumber = model.UserRegistrationModel.PhoneNumber
            };


            var result = await _userManager.CreateAsync(user, model.UserRegistrationModel.Password);

            if (result.Succeeded)
            {

                return RedirectToAction("Login");

            }
            else
            {
                return View();
            }


        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model)
        {

            var result = await _signInManager.PasswordSignInAsync(model.LoginModel.UserName, model.LoginModel.Password, model.LoginModel.RememberMe, false);
            if (result?.Succeeded ?? false)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("", "Invalid user name or password");
                return View();
            }
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);


            if (user == null)
            {
                ViewBag.message = "email sent, please check your inbox";
                return View();
            }
            var confirmationcode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackurl = Url.Action(
         controller: "account",
         action: "resetpassword",
         values: new { userId = user.Id, code = confirmationcode },
         protocol: Request.Scheme);


            MailData mailData = new MailData();
            mailData.EmailSubject = "reset password";
            mailData.EmailBody = callbackurl;
            mailData.EmailToId = email;

            _emailSender.SendMail(mailData);

            ViewBag.message = "email sent, please check your inbox";
            return View();

        }
        public IActionResult ResetPassword(string userId, string code)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            model.UserId = userId;
            model.Code = code;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
          var user = await _userManager.FindByIdAsync(model.UserId);
           await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            return RedirectToAction("Login");
        }


    }
}
