using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Handlers.Resumes;

public sealed class CreateResumeHandler
{
    private readonly CreateResumeRequestValidator _validator;
    private readonly IResumeService _resumeService;
    private readonly ILogger<CreateResumeHandler> _logger;

    public CreateResumeHandler(CreateResumeRequestValidator validator, IResumeService resumeService,
        ILogger<CreateResumeHandler> logger)
    {
        _resumeService = resumeService;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(Guid userId, CreateResumeRequest request, CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var resume = await _resumeService.CreateResumeAsync(userId, request.Resume, ct);

        _logger.LogInformation("User with id {UserId} successfully created resume with id {ResumeId}.",
            userId, resume.Id);
    }
}