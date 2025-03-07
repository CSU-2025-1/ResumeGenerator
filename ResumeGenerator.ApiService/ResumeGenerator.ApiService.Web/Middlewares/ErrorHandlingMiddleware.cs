using System.Net.Mime;
using System.Text.Json;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Results;
using ResumeGenerator.ApiService.Web.Models;

namespace ResumeGenerator.ApiService.Web.Middlewares;

public sealed class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            string newContent;
            ServerErrorModel errorModel;
            switch (ex)
            {
                case ExceptionBase customExceptionBase:
                    context.Response.StatusCode = customExceptionBase.StatusCode;
                    errorModel = new ServerErrorModel(customExceptionBase.Error);
                    _logger.LogError($"[{errorModel.Error.Code}]: {errorModel.Error.Description}");
                    newContent = JsonSerializer.Serialize(errorModel);
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorModel = new ServerErrorModel(
                        new Error(ex.GetType().ToString(), ex.Message));
                    _logger.LogError($"[{errorModel.Error.Code}]: {errorModel.Error.Description}");
                    newContent = JsonSerializer.Serialize(errorModel);
                    break;
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(newContent);
        }
    }
}