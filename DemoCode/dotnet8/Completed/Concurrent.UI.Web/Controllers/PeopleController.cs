using Microsoft.AspNetCore.Mvc;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI.Web.Controllers;

public class PeopleController : Controller
{
    ILogger<PeopleController> logger;
    public PeopleController(ILogger<PeopleController> logger)
    {
        this.logger = logger;
    }

    PersonReader reader = new();

    public Task<IActionResult> WithTask(CancellationToken cancelToken)
    {
        ViewData["Title"] = "Using Task";
        ViewData["RequestStart"] = DateTime.Now;

        Task<List<Person>> peopleTask = reader.GetPeopleAsync(cancelToken);
        Task<IActionResult> result = 
            peopleTask.ContinueWith<IActionResult>(task =>
        {
            if (task.IsFaulted)
            {
                var errors = task.Exception!.Flatten().InnerExceptions;
                logger.LogDebug("Exception");
                return View("Error", errors);
            }
            if (task.IsCanceled)
            {
                logger.LogDebug("Canceled");
                return new EmptyResult();
            }
            List<Person> people = task.Result;
            ViewData["RequestEnd"] = DateTime.Now;
            logger.LogDebug("Done");
            return View("Index", people);
        });
        return result;
    }

    public async Task<IActionResult> WithAwait(CancellationToken cancelToken)
    {
        ViewData["Title"] = "Using async/await";
        ViewData["RequestStart"] = DateTime.Now;

        try
        {
            List<Person> people = await reader.GetPeopleAsync(cancelToken);
            logger.LogDebug("Done");
            return View("Index", people);
        }
        catch (OperationCanceledException)
        {
            logger.LogDebug("Canceled");
            return new EmptyResult();
        }
        catch (Exception ex)
        {
            List<Exception> errors = [ex];
            logger.LogDebug("Exception");
            return View("Error", errors);
        }
        finally
        {
            ViewData["RequestEnd"] = DateTime.Now;
        }
    }
}
