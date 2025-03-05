using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public interface IResumeService
{
    Task<Resume> CreateResumeAsync(CreateResumeRequest request, CancellationToken ct = default);

    Task<List<Resume>> GetAllResumesByUserIdAsync(GetResumesByUserIdRequest request, CancellationToken ct = default);
}