using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Handlers.Resumes;

public class CreateResumeHandler(
    CreateResumeRequestValidator validator,
    IResumeService resumeService,
    ILogger<CreateResumeHandler> logger
    )
{
    public async Task Handle(CreateResumeRequest request, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var resume = await resumeService.CreateResumeAsync(request, ct);
        
        logger.LogInformation($"User with id {request.Resume.UserId} successfully " +
                              $"created wish with id {resume.Id}.");
    }
}