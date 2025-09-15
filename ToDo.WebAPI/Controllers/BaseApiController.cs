using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ToDo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??=
        HttpContext.RequestServices.GetService<IMediator>()!;

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        var problemsFactory = HttpContext.RequestServices
            .GetRequiredService<ProblemDetailsFactory>();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.HasError(error
            => error.Message.Contains("not found", StringComparison.CurrentCultureIgnoreCase)))
        {
            var notFoundDetails = problemsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: StatusCodes.Status404NotFound);
            return NotFound(notFoundDetails);
        }

        var errorDetail = string.Join("; ", result.Errors.Select(e => e.Message));
        var badRequestDetails = problemsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: StatusCodes.Status400BadRequest,
            detail: errorDetail);

        return BadRequest(badRequestDetails);
    }
}
