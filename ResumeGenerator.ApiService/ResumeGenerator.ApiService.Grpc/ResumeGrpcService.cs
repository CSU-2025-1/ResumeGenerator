using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ResumeGenerator.ApiService.Application.Services.Resumes;
using ResumeGenerator.ApiService.Grpc.grpc;

namespace ResumeGenerator.ApiService.Grpc;

public sealed class ResumeGrpcService : ResumeServiceGrpc.ResumeServiceGrpcBase
{
    private readonly IResumeService _resumeService;

    public ResumeGrpcService(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    public override async Task<Empty> UpdateResumeStatus(UpdateResumeStatusRequest request, ServerCallContext context)
    {
        var resumeId = Guid.Parse(request.ResumeId);
        var newStatus = (ResumeGenerator.ApiService.Data.Entities.ResumeStatus)request.NewStatus;

        await _resumeService.UpdateResumeStatusAsync(resumeId, newStatus, context.CancellationToken);

        return new Empty();
    }
}
