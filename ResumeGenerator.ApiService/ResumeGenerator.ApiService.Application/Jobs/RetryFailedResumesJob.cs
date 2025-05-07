using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ResumeGenerator.ApiService.Data.Context;
using ResumeGenerator.ApiService.Data.Entities;
using ResumeGenerator.Common.Contracts;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public class RetryFailedResumesJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<RetryFailedResumesJob> _logger;

    private static readonly TimeSpan RunInterval = TimeSpan.FromHours(6);

    public RetryFailedResumesJob(IServiceProvider serviceProvider, IMapper mapper,
        ILogger<RetryFailedResumesJob> logger)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromHours(6));

        try
        {
            while (await timer.WaitForNextTickAsync(ct))
            {
                await RetryResumeCreating(ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RetryFailedResumesJob caused by {exception}", ex.Message);
        }
    }

    private async Task RetryResumeCreating(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var bus = scope.ServiceProvider.GetRequiredService<IBus>();

        var failedResumes = await dbContext.Resumes
            .Where(r => r.ResumeStatus == ResumeStatus.ResumeMakingFailed)
            .ToListAsync(ct);

        foreach (var resume in failedResumes)
        {
            resume.RetryCount++;

            if (resume.RetryCount >= 5)
            {
                dbContext.Resumes.Remove(resume);
                // TODO - дергать grpc ручку для отправки сообщения об этом пользователю в ТГ
                _logger.LogInformation("Resume with id = {Id} delted after 5 retries", resume.Id);
                continue;
            }

            await bus.Publish(_mapper.Map<CreateResumeCommand>(resume), ct);

            _logger.LogInformation("Retry creating resume with id = {Id}, attempt {Count}", resume.Id,
                resume.RetryCount);
        }

        await dbContext.SaveChangesAsync(ct);
    }
}