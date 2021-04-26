using System.Threading.Tasks;
using IA2Assessment.Models;
using IA2Assessment.Models.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IA2Assessment.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        
        public AccountController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        #region Login

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            return View(new AccountLoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await signInManager.PasswordSignInAsync(accountLoginViewModel.UserName, accountLoginViewModel.Password, accountLoginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(accountLoginViewModel.ReturnUrl) &&
                        Url.IsLocalUrl(accountLoginViewModel.ReturnUrl))
                    {
                        return Redirect(accountLoginViewModel.ReturnUrl);
                    }

                    return RedirectToAction("Manage", "Orders");
                }
            }
            
            ModelState.AddModelError("", "Invalid login attempt!");
            return View();
        }
        
        #endregion

        #region Logout
        
        [HttpGet]
        public IActionResult Signout()
        {
            if (signInManager.IsSignedIn(User))
                signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}