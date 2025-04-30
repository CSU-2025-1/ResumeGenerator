using Grpc.Core;
using ResumeGenerator.Common.Contracts.Grpc;
using ResumeStatus = ResumeGenerator.ApiService.Data.Entities.ResumeStatus;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

using Google.Protobuf.WellKnownTypes;

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
        var newStatus = (ResumeStatus)request.NewStatus;

        await _resumeService.UpdateResumeStatusAsync(resumeId, newStatus, context.CancellationToken);

        return new Empty();
    }
}
