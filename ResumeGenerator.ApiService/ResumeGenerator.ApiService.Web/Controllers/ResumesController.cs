using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;
using ResumeGenerator.ApiService.Application.Handlers.Resumes;
using ResumeGenerator.ApiService.Web.Attributes;

namespace ResumeGenerator.ApiService.Web.Controllers;

[Authorize]
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
        var result = await handler.Handle(new GetResumesByUserIdRequest
        {
            UserId = Guid.Parse(User.Claims.First(x => x.Type is ClaimTypes.NameIdentifier).Value)
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
            ResumeId = resumeId,
            UserId = Guid.Parse(User.Claims.First(x => x.Type is ClaimTypes.NameIdentifier).Value)
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
        request.Resume = request.Resume with
        {
            UserId = Guid.Parse(User.Claims.First(x => x.Type is ClaimTypes.NameIdentifier).Value)
        };
        await handler.Handle(request, ct);

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
            ResumeId = resumeId,
            UserId = Guid.Parse(User.Claims.First(x => x.Type is ClaimTypes.NameIdentifier).Value)
        }, ct);

        return NoContent();
    }
}