namespace Imdb.Controllers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Net;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ExceptionController : ControllerBase
{
    private readonly ILogger<ExceptionController> logger;

    public ExceptionController(ILogger<ExceptionController> logger)
    {
        this.logger = logger;
    }

    [Route("/exception")]
    public IActionResult HandleException()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;

        logger?.LogError(exception, "An exception has occurred while executing the request.");

        return Problem(detail: exception.Message, statusCode: (int)HttpStatusCode.BadRequest);
    }
}
