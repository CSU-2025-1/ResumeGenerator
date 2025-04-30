using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Handlers.Resumes;

public sealed class DeleteResumeByIdHandler
{
    private readonly DeleteResumeByIdRequestValidator _validator;
    private readonly IResumeService _resumeService;
    private readonly ILogger<DeleteResumeByIdHandler> _logger;

    public DeleteResumeByIdHandler(DeleteResumeByIdRequestValidator validator, IResumeService resumeService,
        ILogger<DeleteResumeByIdHandler> logger)
    {
        _resumeService = resumeService;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(DeleteResumeByIdRequest request,
        CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        await _resumeService.DeleteResumeByIdAsync(request.ResumeId, ct);

        _logger.LogInformation("Resume with id:{ResumeId} were successfully deleted.", request.ResumeId);
    }
}