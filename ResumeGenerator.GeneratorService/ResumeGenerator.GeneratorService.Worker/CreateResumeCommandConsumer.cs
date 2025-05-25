using System.Net.Mime;
using MassTransit;
using Minio;
using Minio.DataModel.Args;
using ResumeGenerator.Common.Contracts;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;
using ResumeGenerator.GeneratorService.Grpc.Generated;

namespace ResumeGenerator.GeneratorService.Worker;

public sealed class CreateResumeCommandConsumer : IConsumer<CreateResumeCommand>
{
    private static readonly BucketExistsArgs ExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
    private static readonly MakeBucketArgs MakeArgs = new MakeBucketArgs().WithBucket(BucketName);

    private const string BucketName = "resumes";

    private readonly IMinioClient _minioClient;
    private readonly IResumeGenerator _resumeGenerator;
    private readonly TelegramAdapter.TelegramAdapterClient _telegramClient;
    private readonly ResumeServiceGrpc.ResumeServiceGrpcClient _apiClient;
    private readonly ILogger<CreateResumeCommandConsumer> _logger;

    public CreateResumeCommandConsumer(
        IMinioClient minioClient,
        IResumeGenerator resumeGenerator,
        TelegramAdapter.TelegramAdapterClient telegramClient,
        ResumeServiceGrpc.ResumeServiceGrpcClient apiClient, 
        ILogger<CreateResumeCommandConsumer> logger)
    {
        _minioClient = minioClient;
        _resumeGenerator = resumeGenerator;
        _telegramClient = telegramClient;
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateResumeCommand> context)
    {
        CreateResumeCommand command = context.Message;

        try
        {
            Console.WriteLine($"Consumed {context.MessageId}");
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingInProgress, context.CancellationToken);
            await GenerateResumeToMinioAsync(command, context.CancellationToken);
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingSuccess, context.CancellationToken);
            await SendResumeAsync(command, context.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{ExceptionName}]: {ExceptionMessage}", ex.GetType().Name, ex.Message);
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingFailed, context.CancellationToken);
        }
    }

    private async Task UpdateResumeStatusAsync(CreateResumeCommand command,
        ResumeStatus status,
        CancellationToken ct = default)
    {
        await _apiClient.UpdateResumeStatusAsync(new UpdateResumeStatusRequest
        {
            ResumeId = command.ResumeId.ToString(),
            NewStatus = status
        }, cancellationToken: ct);
    }

    private async Task SendResumeAsync(CreateResumeCommand command, CancellationToken ct = default)
    {
        await _telegramClient.SendResumeAsync(new SendResumeRequest
        {
            ResumeID = command.ResumeId.ToString(),
            UserID = command.UserId.ToString(),
            ResumeTitle = command.ResumeName
        }, cancellationToken: ct);
    }

    private async Task GenerateResumeToMinioAsync(CreateResumeCommand command, CancellationToken ct = default)
    {
        bool exists = await _minioClient.BucketExistsAsync(ExistsArgs, ct);

        if (!exists)
        {
            await _minioClient.MakeBucketAsync(MakeArgs, ct);
        }

        await using Stream memoryStream = GeneratePdfAsync(command);
        memoryStream.Seek(0, SeekOrigin.Begin);

        await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{command.ResumeId}.pdf")
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(MediaTypeNames.Application.Pdf),
            ct);
    }

    private Stream GeneratePdfAsync(CreateResumeCommand command) =>
        _resumeGenerator.GeneratePdf(new Resume
        {
            ResumeId = command.ResumeId,
            UserId = command.UserId,
            FirstName = command.UserFirstName,
            LastName = command.UserLastName,
            MiddleName = command.UserPatronymic,
            DesiredPosition = command.DesiredPosition,
            GitHubLink = command.GitHubLink,
            TelegramLink = command.TelegramLink,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Education = command.Education,
            ExperienceYears = command.ExperienceYears,
            HardSkills = command.HardSkills,
            SoftSkills = command.SoftSkills,
        });
}
