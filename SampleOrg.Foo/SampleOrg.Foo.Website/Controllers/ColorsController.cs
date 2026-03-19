using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.Foo.Website.Infrastructure;
using SampleOrg.Foo.Website.Models;

namespace SampleOrg.Foo.Website.Controllers;

[Authorize]
[Route("colors")]
[SimulatedDelay(MedianMs = 60)]
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
    [SimulatedDelay(MedianMs = 1200, Sigma = 0.9)]
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

    [HttpGet("{slug}/subjects/{id:int}")]
    public IActionResult SubjectDetail(string slug, int id)
    {
        var color = AllColors.FirstOrDefault(c => c.Slug == slug);
        if (color is null) return NotFound();

        var subject = SubjectData.AllSubjects.FirstOrDefault(s => s.Id == id);
        if (subject is null) return NotFound();

        var roleIndex = Array.IndexOf(subject.ColorSlugs, slug);
        if (roleIndex < 0) return NotFound();

        var otherColors = subject.ColorSlugs
            .Where(s => s != slug)
            .Select(s => ColorData.BySlug.TryGetValue(s, out var c) ? c : null)
            .Where(c => c is not null)
            .Cast<ColorEntry>()
            .ToArray();

        var otherSubjects = SubjectData.AllSubjects
            .Where(s => s.Id != id && s.ColorSlugs.Contains(slug))
            .OrderBy(s => s.Name)
            .Take(4)
            .ToArray();

        return View((color, subject, roleIndex, otherColors, otherSubjects));
    }
}
