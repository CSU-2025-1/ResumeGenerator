using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace ResumeGenerator.ApiService.Web.ActionFilters;

public sealed class IdempotencyActionFilter : IAsyncActionFilter
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<IdempotencyActionFilter> _logger;

    public IdempotencyActionFilter(IDistributedCache cache, ILogger<IdempotencyActionFilter> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        try
        {
            var request = context.HttpContext.Request;
            request.EnableBuffering();

            using var reader = new StreamReader(
                stream: request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true
            );

            string body = await reader.ReadToEndAsync(context.HttpContext.RequestAborted);
            request.Body.Position = 0;

            string hashedBody = HashBody(body);
            if (await _cache.GetStringAsync(hashedBody, context.HttpContext.RequestAborted) is not null)
            {
                _logger.LogInformation("[{Location}] Cache hit for request {RequestID}",
                    nameof(IdempotencyActionFilter), context.HttpContext.TraceIdentifier);
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            await _cache.SetStringAsync(
                key: hashedBody,
                value: true.ToString(),
                options: new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(10),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3)
                }, context.HttpContext.RequestAborted);
            await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    private static string HashBody(string body)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(body)));
}