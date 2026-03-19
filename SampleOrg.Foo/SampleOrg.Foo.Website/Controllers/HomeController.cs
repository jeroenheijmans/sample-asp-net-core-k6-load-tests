using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.Foo.Website.Models;

namespace SampleOrg.Foo.Website.Controllers;

public class HomeController : Controller
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
