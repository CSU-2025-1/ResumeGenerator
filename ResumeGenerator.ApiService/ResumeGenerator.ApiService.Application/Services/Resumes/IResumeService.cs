using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public interface IResumeService
{
    Task<Resume> CreateResumeAsync(Guid userId, ResumeDto resume, CancellationToken ct = default);

    Task UpdateResumeStatusAsync(Guid resumeId, ResumeStatus newStatus, CancellationToken ct = default);

    Task<List<Resume>> GetAllResumesByUserIdAsync(Guid userId, CancellationToken ct = default);

    Task<Resume> GetResumeByIdAsync(Guid resumeId, CancellationToken ct = default);

    Task DeleteResumeByIdAsync(Guid resumeId, CancellationToken ct = default);
}