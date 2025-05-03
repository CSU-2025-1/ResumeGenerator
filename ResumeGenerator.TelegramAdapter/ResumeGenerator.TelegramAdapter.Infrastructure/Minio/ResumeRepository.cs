using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using ResumeGenerator.TelegramAdapter.Core.Abstractions;

namespace ResumeGenerator.TelegramAdapter.Infrastructure.Minio;

public sealed class ResumeRepository : IResumeRepository
{
    private readonly IMinioClient _minioClient;
    private readonly MinioConfig _config;

    public ResumeRepository(IMinioClient minioClient, IOptions<MinioConfig> minioConfig)
    {
        _minioClient = minioClient;
        _config = minioConfig.Value;
    }

    public async Task<Maybe<MemoryStream>> GetResumeByIdAsync(Guid id, CancellationToken ct = default)
    {
        var ms = new MemoryStream();

        bool bucketExists = await _minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(_config.BucketName),
            cancellationToken: ct);
        if (!bucketExists)
        {
            return Maybe<MemoryStream>.None;
        }

        try
        {
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                    .WithBucket(_config.BucketName)
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