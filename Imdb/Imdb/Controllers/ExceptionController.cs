using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Imdb.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ExceptionController(ILogger<ExceptionController> logger) : ControllerBase
{
    private readonly ILogger<ExceptionController> logger = logger;

    [Route("/exception")]
    public IActionResult HandleException()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
        var statusCode = HttpContext.Response.StatusCode;

        logger?.LogError(exception, "An exception has occurred while executing the request.");

        return Problem(detail: exception.Message, statusCode: statusCode);
    }
}
