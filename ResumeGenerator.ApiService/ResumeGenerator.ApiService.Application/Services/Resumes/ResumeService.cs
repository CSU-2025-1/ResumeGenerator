using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Context;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public sealed class ResumeService : IResumeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ResumeService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Resume> CreateResumeAsync(ResumeDto resume, CancellationToken ct = default)
    {
        var newResume = _mapper.Map<Resume>(resume);
        _context.Resumes.Add(newResume);
        await _context.SaveChangesAsync(ct);

        return newResume;
    }

    public Task<List<Resume>> GetAllResumesByUserIdAsync(Guid userId, CancellationToken ct = default)
        => _context.Resumes
            .Where(resume => resume.UserId == userId)
            .ToListAsync(ct);
}