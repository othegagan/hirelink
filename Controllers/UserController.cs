using System.Net;
using System.Security.Claims;
using hirelink.DbContext;
using hirelink.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;


namespace hirelink.Controllers {
    public class UserController : Controller {

        private readonly HirelinkDbContext _db;

        public UserController(HirelinkDbContext db) {
            _db = db;
        }



        public IActionResult Index() {
            return View();
        }

        [HttpGet("/login")]
        public IActionResult Login() {
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated) {
                // User is already authenticaasdklfaksted, redirect to '/jobs' page
                return Redirect("/jobs");
            }

            return View();
        }

        [HttpPost("/login")]
        public IActionResult Login(User modal) {
            string userName = modal.Username;
            string password = modal.Password;

            if (ModelState.IsValid) {
                // Check for username
                User? user = _db.VerifyCredentials(userName);
                if (user != null) {
                    // Check for password using BCrypt
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);
                    if (isPasswordCorrect) {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) },
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", userName);
                        HttpContext.Session.SetString("Name", user.Name);
                        HttpContext.Session.SetString("Role", user.Role);

                        // Redirect the user to '/jobs' page
                        return Redirect("/jobs");
                    }
                }

                TempData["errorMessage"] = "Invalid credentials, try again";
                return View(modal);
            } else {
                return View(modal);
            }
        }

        [HttpGet("/logout")]
        public IActionResult Logout() {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookie in storedCookies) {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Login", "User");
        }

    }
}
