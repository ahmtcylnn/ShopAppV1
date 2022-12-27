﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;

        }
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName,
            };

            var result= await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
            {
                // generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail","Account", new
                {
                    userId=user.Id,
                    token= code
                });
                // send email
                await _emailSender.SendEmailAsync(model.Email, "Hesabınızı Onaylayınız",$"Lütfen Email Hesabınızı Onaylamak İçin Linke <a href='http://localhost:44311{callbackUrl}'>Tıklayınız</a>");
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Bilinmeyen Hata Oluştu Lütfen Tekrar Deneyiniz");
            return View(model);
        }
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            

            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var user= await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap oluşturulmamış.");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen Hesabınızı Email ile Doğrulayınız");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password,true,false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/"); 
            }
            ModelState.AddModelError("", "Email veya parola yanlış.");
            return View(model);
            
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (userId==null || token==null)
            {
                TempData["Message"] = "Geçersiz Token.";
                return View();
            }
            var user=await _userManager.FindByIdAsync(userId);

            if (user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabınız Onaylandı.";
                    return View();
                }

            }
            TempData["message"] = "Hesabınız Onaylanmadı !";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();

            }
            var user= await _userManager.FindByEmailAsync(Email);
            if (user==null)
            {
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            // generate token
            
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code
            });
            // send email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı Yenilemek İçin Linke <a href='http://localhost:44311{callbackUrl}'>Tıklayınız</a>");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ResetPassword(string userId,string token)
        {
            if (token==null)
            {
                return RedirectToAction("Home", "Index");
            }
            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user==null)
            {
                return RedirectToAction("Home", "Index");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);

           
        }

    }
}
