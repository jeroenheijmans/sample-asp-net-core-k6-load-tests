using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.Foo.Website.Models;

namespace SampleOrg.Foo.Website.Controllers;

[Authorize]
[Route("colors")]
public class ColorsController : Controller
{
    // Delegates to the shared ColorData cache — same data available to SubjectsController.
    private static readonly ColorEntry[] AllColors = ColorData.AllColors;

    [HttpGet("")]
    public IActionResult Index() => View(AllColors);

    [HttpGet("{slug}")]
    public IActionResult Detail(string slug)
    {
        var color = AllColors.FirstOrDefault(c => c.Slug == slug);
        if (color is null) return NotFound();
        return View(color);
    }

    [HttpGet("{slug}/subjects")]
    public IActionResult Subjects(string slug)
    {
        var color = AllColors.FirstOrDefault(c => c.Slug == slug);
        if (color is null) return NotFound();
        var subjects = SubjectData.AllSubjects
            .Where(s => s.ColorSlugs.Contains(slug))
            .OrderBy(s => s.Name)
            .ToArray();
        return View((color, subjects));
    }
}
