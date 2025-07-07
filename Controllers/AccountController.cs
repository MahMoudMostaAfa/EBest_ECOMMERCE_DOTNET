using AspNetCoreGeneratedDocument;
using EBest.Models;
using EBest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EBest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , IConfiguration configuration
            )
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.configuration = configuration;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Register( RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var user = new ApplicationUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Address = registerDto.Address,
                PhoneNumber = registerDto.PhoneNumber ?? ""
            };

            var result=  await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                // Add user to the "client" role
                await _userManager.AddToRoleAsync(user, "client");
                // Sign in the user after registration
                await _signInManager.SignInAsync(user, false);


            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerDto);
            }

           return RedirectToAction("Index","Home");
        }


        
        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                
            }
          
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login( LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginDto.Email,
               loginDto.Password,
               loginDto.RememberMe,
               false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }else
            {
                ViewBag.ErrorMessage = "Invalid login attempt. Please check your email and password.";
            }




             return View(loginDto);

        }

        [Authorize]
        public async Task< IActionResult >Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var profileDto = new ProfileDto
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                Email = user!.Email!,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
            return View(profileDto);
        }

        [Authorize]
        [HttpPost]
        public async Task< IActionResult> Profile( ProfileDto profileDto)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please update all requird fields";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied");
            }
            user.FirstName = profileDto.FirstName;
            user.LastName = profileDto.LastName;
            user.Email = profileDto.Email;
            user.UserName=profileDto.Email;
            user.PhoneNumber = profileDto.PhoneNumber;
            user.Address = profileDto.Address;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) {
                ViewBag.SuccessMessage = "Profile updated successfully.";
                return View();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(profileDto);
        }

        [Authorize]
        public IActionResult Password()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Password(PasswordDto passwordDto)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("AccessDenied");
            }
            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Password updated successfully!"; 
                
            }
            else
            {
                ViewBag.ErrorMessage = "Error: " + result.Errors.First().Description;
            }

           



            return View();
        }


          
        public IActionResult ForgotPassword()
        {

            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
                
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([Required,EmailAddress] string email)
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Email = email;

            if (!ModelState.IsValid)
            {
                ViewBag.EmailError = ModelState["email"]?.Errors.First().ErrorMessage ?? "Invaild email address";

            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user!= null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string url = Url.ActionLink("ResetPassword", "Account", new
                {
                    token,
                }) ?? "url error";
             
                MailSender.SendMail( configuration["BrevoSettings:SenderName"], configuration["BrevoSettings:SenderEmail"],user.Email,user.FirstName,url );
                Console.WriteLine($"url is : {url}");
            }

            ViewBag.SuccessMessage= " you will receive a password reset link shortly.";

            return View();

        }



        public  IActionResult ResetPassword( string? token)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string? token, PasswordResetDto passwordResetDto)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if(!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please fill all required fields";
                return View(passwordResetDto);
            }
            var user = await _userManager.FindByEmailAsync(passwordResetDto.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Token not valid!";
                return View(passwordResetDto);
            }

            var result = await _userManager.ResetPasswordAsync(user, token, passwordResetDto.Password);

            if (result.Succeeded)
            {
                ViewBag.SuccessMessage = "Password reset successfully!";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(passwordResetDto);

        }
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }
    }

}
