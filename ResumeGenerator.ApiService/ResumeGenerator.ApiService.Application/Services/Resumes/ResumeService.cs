using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Application.DTO.Requests.Resumes;
using ResumeGenerator.ApiService.Data.Context;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public class ResumeService(AppDbContext context, IMapper mapper) : IResumeService
{
    public async Task<Resume> CreateResumeAsync(CreateResumeRequest request, CancellationToken ct = default)
    {
        var newResume = mapper.Map<Resume>(request.Resume);
        await context.Resumes.AddAsync(newResume, ct);
        return newResume;
    }

    public Task<List<Resume>> GetAllResumesByUserIdAsync(GetResumesByUserIdRequest request,
        CancellationToken ct = default)
        => context.Resumes
            .Where(resume => resume.UserId == request.UserId)
            .ToListAsync(ct);
}