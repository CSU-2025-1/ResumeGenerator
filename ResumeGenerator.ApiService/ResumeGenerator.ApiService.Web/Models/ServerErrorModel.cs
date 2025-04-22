using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Web.Models;

public sealed class ServerErrorModel
{
    public Error Error { get; init; }

    public ServerErrorModel(Error error)
    {
        Error = error;
    }
}