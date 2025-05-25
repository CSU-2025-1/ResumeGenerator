using Microsoft.AspNetCore.Http;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Results;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Application.Services.TelegramAdapter;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Handlers.Resumes;

public sealed class ResendResumeByIdHandler
{
    private readonly IResumeService _resumeService;
    private readonly ITelegramAdapter _telegramAdapter;

    public ResendResumeByIdHandler(IResumeService resumeService, ITelegramAdapter telegramAdapter)
    {
        _resumeService = resumeService;
        _telegramAdapter = telegramAdapter;
    }

    public async Task HandleAsync(ResendResumeByIdRequest request, CancellationToken ct = default)
    {
        var resume = await _resumeService.GetResumeByIdAsync(request.ResumeId, request.CurrentUserId, ct);
        if (resume.ResumeStatus != ResumeStatus.ResumeMakingSuccess)
        {
            throw new UnprocessableEntityException
            {
                Error = new Error(StatusCodes.Status422UnprocessableEntity.ToString(),
                    "Can resend only successfully made resume")
            };
        }

        await _telegramAdapter.SendResumeToUserAsync(resume, ct);
    }
}