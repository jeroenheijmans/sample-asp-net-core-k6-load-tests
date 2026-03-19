using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.Foo.Website.Infrastructure;

namespace SampleOrg.Foo.Website.Controllers;

[SimulatedDelay(MedianMs = 80)]
public class UserController : Controller
{
    [HttpGet("/user")]
    public IActionResult Index(string? returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost("/user")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Index(string? returnUrl, string username, string password)
    {
        if (username != "bart" || password != "secret")
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Error"] = "Invalid username or password.";
            Response.StatusCode = 401;
            return View();
        }

        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        var destination = Url.IsLocalUrl(returnUrl) ? returnUrl : "/";
        return Redirect(destination);
    }

    [HttpGet("/user/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}
