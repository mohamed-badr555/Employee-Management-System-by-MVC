using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> Signinmanager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinmanager)
        {
            this.userManager = userManager;
            Signinmanager = signinmanager;
        }

        #region SignUp

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(viewModel.UserName);
                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = viewModel.UserName,
                        Email = viewModel.Email,
                        IsAgree = viewModel.ISAgree,
                        FirstName = viewModel.FName,
                        LastName = viewModel.FName,
                    };
                    var result = await userManager.CreateAsync(user, viewModel.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                ModelState.AddModelError(string.Empty, "User Name is already taken");
            }

            return View(viewModel);
        }

        #endregion
        #region SignIn

        public IActionResult SignIn()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await Signinmanager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }

                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        #endregion
        #region Sign Out
        public async new Task<IActionResult> SignOut()
        {
            await Signinmanager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }


        #endregion
        //ForgetPassword
        #region ForgetPassword
        public IActionResult ForgetPassword()
        {

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ResendForgetEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var link = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                    var email = new Email()
                    {
                        Subject = "Reset Your Password",
                        Body = link,
                        Recipiens = model.Email
                    };
                    EmailSettings.sendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));

                }
                ModelState.AddModelError(string.Empty, "Invalid Email Address");
            }
            return View(model);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion
        #region Reset Password
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"].ToString();
                var token = TempData["token"].ToString();
                var user = await userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Attempt");
            }
            return View(model);
        }
        #endregion
    }
}
