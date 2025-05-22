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
    private readonly GetResumesByUserIdRequestValidator _validator;
    private readonly IResumeService _resumeService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateResumeHandler> _logger;

    public GetAllResumesByUserIdHandler(GetResumesByUserIdRequestValidator validator, IResumeService resumeService,
        IMapper mapper, ILogger<CreateResumeHandler> logger)
    {
        _mapper = mapper;
        _resumeService = resumeService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GetResumesByUserIdResponse> Handle(GetResumesByUserIdRequest request,
        CancellationToken ct = default)
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        BadRequestException.ThrowByValidationResult(validationResult);

        var resumes = await _resumeService.GetAllResumesByUserIdAsync(request.UserId,
            request.PageNumber, request.PageSize, ct);

        _logger.LogInformation("Resumes of user with id {UserId} were successfully sent to him.", request.UserId);

        return new GetResumesByUserIdResponse
        {
            Resumes = resumes
                .Select(_mapper.Map<ShortResumeDto>).ToArray()
        };
    }
}