using System.Net.Mime;
using MassTransit;
using Minio;
using Minio.DataModel.Args;
using ResumeGenerator.Common.Contracts;
using ResumeGenerator.GeneratorService.Core.Entities;
using ResumeGenerator.GeneratorService.Core.Interfaces;

namespace ResumeGenerator.GeneratorService.Worker;

public sealed class CreateResumeCommandConsumer : IConsumer<CreateResumeCommand>
{
    private static readonly PdfParameters Parameters =
        new(148, 210, 100, 100, 4, 4, 4, 4);

    private static readonly BucketExistsArgs ExistsArgs = new BucketExistsArgs().WithBucket(BucketName);
    private static readonly MakeBucketArgs MakeArgs = new MakeBucketArgs().WithBucket(BucketName);

    private const string BucketName = "resumes";

    private readonly IMinioClient _minioClient;
    private readonly IResumeGenerator _resumeGenerator;

    public CreateResumeCommandConsumer(IMinioClient minioClient, IResumeGenerator resumeGenerator)
    {
        _minioClient = minioClient;
        _resumeGenerator = resumeGenerator;
    }

    public async Task Consume(ConsumeContext<CreateResumeCommand> context)
    {
        CreateResumeCommand command = context.Message;

        bool exists = await _minioClient.BucketExistsAsync(ExistsArgs, context.CancellationToken);

        if (!exists)
        {
            await _minioClient.MakeBucketAsync(MakeArgs, context.CancellationToken);
        }

        using var memoryStream = new MemoryStream(GeneratePdf(command));

        await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject($"{command.ResumeId}.pdf")
                .WithStreamData(memoryStream)
                .WithObjectSize(memoryStream.Length)
                .WithContentType(MediaTypeNames.Application.Pdf),
            context.CancellationToken);
    }

    private byte[] GeneratePdf(CreateResumeCommand command) => _resumeGenerator.GeneratePdf(new Resume
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
    }, Parameters);
}
