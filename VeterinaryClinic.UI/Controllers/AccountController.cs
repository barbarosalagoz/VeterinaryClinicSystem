using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VeterinaryClinic.UI.Models.Account;
using VeterinaryClinic.UI.Services;

namespace VeterinaryClinic.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiClient _authApiClient;

        public AccountController(IAuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var auth = await _authApiClient.LoginAsync(model.Email, model.Password);

            if (auth is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            // 1. Kullanıcı Kimlik Bilgileri (Ad, Email, Rol)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(auth.FullName) ? auth.Email : auth.FullName),
                new Claim(ClaimTypes.Email, auth.Email),
                new Claim(ClaimTypes.Role, auth.Role)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            // 2. KRİTİK DÜZELTME: Token'ı Claim yerine "Properties" içine saklıyoruz.
            // Bu sayede 'GetTokenAsync' metodu token'ı bulabilecek.
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Beni hatırla mantığı için
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            };

            // Token'ı güvenli alana (Token Store) kaydediyoruz
            authProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = "access_token", // Handler bu isimle arıyor
                    Value = auth.Token
                }
            });

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties); // Properties'i buraya parametre olarak geçiyoruz

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}