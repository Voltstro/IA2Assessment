using System.Threading.Tasks;
using IA2Assessment.Models;
using IA2Assessment.Models.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IA2Assessment.Controllers
{
    /// <summary>
    ///     <see cref="Controller"/> for accounts
    /// </summary>
    public class AccountController : Controller
    {
        private readonly SignInManager<User> signInManager;
        
        public AccountController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        #region Login

        /// <summary>
        ///     Gets the login view
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            return View(new AccountLoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        ///     Starts the login process
        /// </summary>
        /// <param name="accountLoginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                //Try to sign in with our sign in manager
                SignInResult result = await signInManager.PasswordSignInAsync(accountLoginViewModel.UserName, accountLoginViewModel.Password, accountLoginViewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    //If we have a return url, go to it
                    if (!string.IsNullOrEmpty(accountLoginViewModel.ReturnUrl) &&
                        Url.IsLocalUrl(accountLoginViewModel.ReturnUrl))
                    {
                        return Redirect(accountLoginViewModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            
            //We fail to login in
            ModelState.AddModelError("", "Invalid login attempt!");
            return View();
        }
        
        #endregion

        #region Logout
        
        /// <summary>
        ///     Starts the log out process
        /// </summary>
        /// <returns></returns>
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