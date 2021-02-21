using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalLifeApp.Models;
using DigitalLifeApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLifeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Name,
                Email = registerVM.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(appUser, Helpers.Helper.Roles.User.ToString());
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("List", "Invoice");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(InvoiceController.List), "Invoice");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View(login);

            AppUser loginUser = await _userManager.FindByEmailAsync(login.Email);

            if (loginUser == null)
            {
                ModelState.AddModelError("", "Email ve ya password dogru deyil!");
                return View(login);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(loginUser, login.Password, false, true);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email ve ya password dogru deyil!");
                return View(login);
            }

            return RedirectToAction("List", "Invoice");

        }

        public async Task CreateRole()
        {
            if (!(await _roleManager.RoleExistsAsync(Helpers.Helper.Roles.Admin.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helpers.Helper.Roles.Admin.ToString()));
            }

            if (!(await _roleManager.RoleExistsAsync(Helpers.Helper.Roles.User.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helpers.Helper.Roles.User.ToString()));
            }
        }
    }
}
