using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;
using ResumeGenerator.ApiService.Application.Handlers.Resumes;

namespace ResumeGenerator.ApiService.Web.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class ResumesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetResumesByUserIdResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetResumesByUserIdResponse>> GetAllResumesByUserId(
        [FromQuery] Guid userId,
        [FromServices] GetAllResumesByUserIdHandler handler,
        CancellationToken ct = default)
    {
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
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> CreateResume(
        [FromBody] CreateResumeRequest request,
        [FromServices] CreateResumeHandler handler,
        CancellationToken ct = default)
    {
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
            ResumeId = resumeId
        }, ct);
        
        return NoContent();
    }
}