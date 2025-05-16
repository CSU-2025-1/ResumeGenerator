using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Handlers.Resumes;
using ResumeGenerator.ApiService.Application.Results;
using ResumeGenerator.ApiService.Web.Attributes;

namespace ResumeGenerator.ApiService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class ResumesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetResumesByUserIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetResumesByUserIdResponse>> GetAllResumesByUserId(
        [FromServices] GetAllResumesByUserIdHandler handler,
        CancellationToken ct = default)
    {
        var userId = (Guid)HttpContext.Items["UserId"];
        NotFoundException.ThrowIfNull(userId,
            new Error(StatusCodes.Status404NotFound.ToString(), $"User with id: {userId} not found in database."));
        var result = await handler.Handle(new GetResumesByUserIdRequest
        {
            UserId = userId
        }, ct);

        return Ok(result);
    }

    [HttpGet("{resumeId:guid}")]
    [ProducesResponseType(typeof(GetResumeByIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetResumeByIdResponse>> GetResumeById(
        [FromRoute] Guid resumeId,
        [FromServices] GetResumeByIdHandler handler,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(new GetResumeByIdRequest
        {
            ResumeId = resumeId
        }, ct);

        return Ok(result);
    }

    [HttpPost]
    [Idempotent]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> CreateResume(
        [FromBody] CreateResumeRequest request,
        [FromServices] CreateResumeHandler handler,
        CancellationToken ct = default)
    {
        var userId = (Guid)HttpContext.Items["UserId"];
        NotFoundException.ThrowIfNull(userId,
            new Error(StatusCodes.Status404NotFound.ToString(), $"User with id: {userId} not found in database."));

        
        await handler.Handle(request with { Resume = request.Resume with { UserId = userId } } , ct);
        return Accepted();
    }

    [HttpDelete("{resumeId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteResumeById(
        [FromRoute] Guid resumeId,
        [FromServices] DeleteResumeByIdHandler handler,
        CancellationToken ct = default)
    {
        await handler.Handle(new DeleteResumeByIdRequest
        {
            ResumeId = resumeId
        }, ct);

        return NoContent();
    }
}