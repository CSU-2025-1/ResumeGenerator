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
    public Task<GetResumesByUserIdResponse> GetAllResumesByUserId(
        [FromQuery] Guid userId,
        [FromServices] GetAllResumesByUserIdHandler handler,
        CancellationToken ct = default)
        => handler.Handle(new GetResumesByUserIdRequest
        {
            UserId = userId
        }, ct);

    [HttpPost]
    public Task CreateResume(
        [FromBody] CreateResumeRequest request,
        [FromServices] CreateResumeHandler handler,
        CancellationToken ct = default)
        => handler.Handle(request, ct);
}