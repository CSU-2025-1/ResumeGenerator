using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public interface IResumeService
{
    Task<Resume> CreateResumeAsync(ResumeDto resume, CancellationToken ct = default);

    Task<List<Resume>> GetAllResumesByUserIdAsync(Guid userId, CancellationToken ct = default);
}