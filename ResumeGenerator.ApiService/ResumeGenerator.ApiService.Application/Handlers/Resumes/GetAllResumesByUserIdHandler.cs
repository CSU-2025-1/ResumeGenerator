using AutoMapper;
using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.DTO.Responses.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Validators.Resumes;

namespace ResumeGenerator.ApiService.Application.Handlers.Resumes;

public sealed class GetAllResumesByUserIdHandler
{
    private readonly GetResumesByUserIdRequestValidator validator;
    private readonly IResumeService resumeService;
    private readonly IMapper mapper;
    private readonly ILogger<CreateResumeHandler> logger;

    public GetAllResumesByUserIdHandler(GetResumesByUserIdRequestValidator validator, IResumeService resumeService,
        IMapper mapper, ILogger<CreateResumeHandler> logger)
    {
        this.mapper = mapper;
        this.resumeService = resumeService;
        this.validator = validator;
        this.logger = logger;
    }

    public async Task<GetResumesByUserIdResponse> Handle(GetResumesByUserIdRequest request,
        CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var resumes = await resumeService.GetAllResumesByUserIdAsync(request.UserId, ct);

        logger.LogInformation($"Resumes of user with id {request.UserId} were successfully sent to him.");
        return new GetResumesByUserIdResponse
        {
            Resumes = resumes
                .Select(mapper.Map<ResumeDto>).ToArray()
        };
    }
}