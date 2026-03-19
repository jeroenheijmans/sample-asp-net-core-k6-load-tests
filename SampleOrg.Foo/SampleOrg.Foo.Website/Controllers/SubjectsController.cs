using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleOrg.Foo.Website.Infrastructure;
using SampleOrg.Foo.Website.Models;

namespace SampleOrg.Foo.Website.Controllers;

[Authorize]
[Route("subjects")]
[SimulatedDelay(MedianMs = 70)]
public class SubjectsController : Controller
{
    private static readonly SubjectEntry[] AllSubjects = SubjectData.AllSubjects;

    [HttpGet("")]
    public IActionResult Index()
        => View((AllSubjects, ColorData.BySlug));

    [HttpGet("{id:int}")]
    public IActionResult Detail(int id)
    {
        var subject = AllSubjects.FirstOrDefault(s => s.Id == id);
        if (subject is null) return NotFound();
        var colors = subject.ColorSlugs
            .Select(slug => ColorData.BySlug.TryGetValue(slug, out var c) ? c : null)
            .ToArray();
        return View((subject, colors));
    }
}
