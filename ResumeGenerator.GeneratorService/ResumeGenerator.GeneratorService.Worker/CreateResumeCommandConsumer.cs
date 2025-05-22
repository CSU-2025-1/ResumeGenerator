using System.Net.Mime;
using Google.Protobuf.WellKnownTypes;
using HtmlToPdf2AZ.Models;
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
    private static readonly MarginOptions MarginOptions = new("0.15in");
    private static readonly PaperFormat PaperFormat = PaperFormat.A4;

    private static readonly BucketExistsArgs ExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
    private static readonly MakeBucketArgs MakeArgs = new MakeBucketArgs().WithBucket(BucketName);

    private const string BucketName = "resumes";

    private readonly IMinioClient _minioClient;
    private readonly IResumeGenerator _resumeGenerator;
    private readonly TelegramAdapter.TelegramAdapterClient _telegramClient;
    private readonly ResumeServiceGrpc.ResumeServiceGrpcClient _apiClient;

    public CreateResumeCommandConsumer(
        IMinioClient minioClient,
        IResumeGenerator resumeGenerator,
        TelegramAdapter.TelegramAdapterClient telegramClient,
        ResumeServiceGrpc.ResumeServiceGrpcClient apiClient)
    {
        _minioClient = minioClient;
        _resumeGenerator = resumeGenerator;
        _telegramClient = telegramClient;
        _apiClient = apiClient;
    }

    public async Task Consume(ConsumeContext<CreateResumeCommand> context)
    {
        CreateResumeCommand command = context.Message;

        try
        {
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingInProgress, context.CancellationToken);
            await GenerateResumeToMinioAsync(command, context.CancellationToken);
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingSuccess, context.CancellationToken);
            await SendResumeAsync(command, context.CancellationToken);
        }
        catch (Exception)
        {
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingFailed, context.CancellationToken);
        }
    }

    private Task<Empty> UpdateResumeStatusAsync(
        CreateResumeCommand command, ResumeStatus status, CancellationToken ct = default) =>
            _apiClient.UpdateResumeStatusAsync(new UpdateResumeStatusRequest
            {
                ResumeId = command.ResumeId.ToString(),
                NewStatus = status
            }, cancellationToken: ct).ResponseAsync;

    private Task<Empty> SendResumeAsync(CreateResumeCommand command, CancellationToken ct = default) =>
        _telegramClient.SendResumeAsync(new SendResumeRequest
        {
            ResumeID = command.ResumeId.ToString(),
            UserID = command.UserId.ToString(),
            ResumeTitle = command.ResumeName
        }, cancellationToken: ct).ResponseAsync;

    private async Task GenerateResumeToMinioAsync(CreateResumeCommand command, CancellationToken ct = default)
    {
        bool exists = await _minioClient.BucketExistsAsync(ExistsArgs, ct);

        if (!exists)
        {
            await _minioClient.MakeBucketAsync(MakeArgs, ct);
        }

        await using Stream memoryStream = await GeneratePdfAsync(command, ct);

        await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{command.ResumeId}.pdf")
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(MediaTypeNames.Application.Pdf),
            ct);
    }

    private Task<Stream> GeneratePdfAsync(CreateResumeCommand command, CancellationToken ct = default) =>
        _resumeGenerator.GeneratePdfAsync(new Resume
        {
            ResumeId = command.ResumeId,
            UserId = command.UserId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            MiddleName = command.MiddleName,
            DesiredPosition = command.DesiredPosition,
            GitHubLink = command.GitHubLink,
            TelegramLink = command.TelegramLink,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Education = command.Education,
            ExperienceYears = command.ExperienceYears,
            HardSkills = command.HardSkills,
            SoftSkills = command.SoftSkills,
        }, MarginOptions, PaperFormat, ct);
}
