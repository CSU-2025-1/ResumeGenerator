using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Context;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public sealed class ResumeService(AppDbContext context, IMapper mapper) : IResumeService
{
    public async Task<Resume> CreateResumeAsync(ResumeDto resume, CancellationToken ct = default)
    {
        var newResume = mapper.Map<Resume>(resume);
        context.Resumes.Add(newResume);
        await context.SaveChangesAsync(ct);

        return newResume;
    }

    public Task<List<Resume>> GetAllResumesByUserIdAsync(Guid userId, CancellationToken ct = default)
        => context.Resumes
            .Where(resume => resume.UserId == userId)
            .ToListAsync(ct);
}