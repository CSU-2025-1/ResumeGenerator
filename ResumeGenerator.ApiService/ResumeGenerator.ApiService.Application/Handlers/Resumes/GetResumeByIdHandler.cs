using AutoMapper;
using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Handlers.Resumes;

public sealed class GetResumeByIdHandler
{
    private readonly GetResumeByIdRequestValidator _validator;
    private readonly IResumeService _resumeService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateResumeHandler> _logger;

    public GetResumeByIdHandler(GetResumeByIdRequestValidator validator, IResumeService resumeService,
        IMapper mapper, ILogger<CreateResumeHandler> logger)
    {
        _mapper = mapper;
        _resumeService = resumeService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GetResumeByIdResponse> Handle(GetResumeByIdRequest request,
        CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var resume = await _resumeService.GetResumeByIdAsync(request.ResumeId, ct);

        _logger.LogInformation("Resume with id:{ResumeId} were successfully sent to user.", resume.Id);

        return new GetResumeByIdResponse
        {
            Resume = _mapper.Map<ResumeDto>(resume)
        };
    }
}