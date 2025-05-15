using CSharpFunctionalExtensions;
using Minio;
using Minio.DataModel.Args;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Minio;

public sealed class ResumeRepository : IResumeRepository
{
    private const string BucketName = "resumes";
    private readonly IMinioClient _minioClient;

    public ResumeRepository(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<Maybe<MemoryStream>> GetResumeByIdAsync(Guid id, CancellationToken ct = default)
    {
        var ms = new MemoryStream();

        bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(BucketName),
            cancellationToken: ct);
        if (!bucketExists)
        {
            return Maybe<MemoryStream>.None;
        }

        try
        {
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject($"{id}.pdf")
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                    }),
                cancellationToken: ct);

            return ms;
        }
        catch
        {
            return Maybe<MemoryStream>.None;
        }
    }
}