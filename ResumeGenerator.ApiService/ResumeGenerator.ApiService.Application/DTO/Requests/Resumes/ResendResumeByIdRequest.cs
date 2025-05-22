namespace ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;

public sealed record ResendResumeByIdRequest(
    Guid ResumeId,
    Guid CurrentUserId
);