using ResumeGenerator.ApiService.Application.Results;

namespace ResumeGenerator.ApiService.Web.Models;

public class ServerErrorModel
{
    public ServerErrorModel(Error error)
    {
        Error = error;
    }

    public Error Error { get; init; }
}