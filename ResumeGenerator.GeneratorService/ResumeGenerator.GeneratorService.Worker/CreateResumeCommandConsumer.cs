using System.Net.Mime;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingInProgress);
            await GenerateResumeToMinioAsync(context, command);
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingSuccess);
            await SendResumeAsync(command);
        }
        catch (Exception)
        {
            await UpdateResumeStatusAsync(command, ResumeStatus.ResumeMakingFailed);
        }
    }

    private Task<Empty> UpdateResumeStatusAsync(CreateResumeCommand command, ResumeStatus status) =>
        _apiClient.UpdateResumeStatusAsync(new UpdateResumeStatusRequest
        {
            ResumeId = command.ResumeId.ToString(),
            NewStatus = status
        }).ResponseAsync;

    private Task<Empty> SendResumeAsync(CreateResumeCommand command) =>
        _telegramClient.SendResumeAsync(new SendResumeRequest
        {
            ResumeID = command.ResumeId.ToString(),
            UserID = command.UserId.ToString()
        }).ResponseAsync;

    private async Task GenerateResumeToMinioAsync(
        ConsumeContext<CreateResumeCommand> context, CreateResumeCommand command)
    {
        bool exists = await _minioClient.BucketExistsAsync(ExistsArgs, context.CancellationToken);

        if (!exists)
        {
            await _minioClient.MakeBucketAsync(MakeArgs, context.CancellationToken);
        }

        await using Stream memoryStream = await GeneratePdfAsync(command);

        await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{command.ResumeId}.pdf")
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(MediaTypeNames.Application.Pdf),
            context.CancellationToken);
    }

    private Task<Stream> GeneratePdfAsync(CreateResumeCommand command) => _resumeGenerator.GeneratePdfAsync(new Resume
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
    }, MarginOptions, PaperFormat);
}
