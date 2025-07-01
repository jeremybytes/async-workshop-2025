using Microsoft.AspNetCore.Mvc;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI.Web.Controllers;

public class PeopleController : Controller
{
    PersonReader reader = new();

    public ViewResult WithTask()
    {
        ViewData["Title"] = "Using Task";
        ViewData["RequestStart"] = DateTime.Now;

        List<Person> people = [];

        ViewData["RequestEnd"] = DateTime.Now;
        return View("Index", people);
    }

    public ViewResult WithAwait()
    {
        ViewData["Title"] = "Using async/await";
        ViewData["RequestStart"] = DateTime.Now;

        List<Person> people = [];

        ViewData["RequestEnd"] = DateTime.Now;
        return View("Index", people);
    }
}
