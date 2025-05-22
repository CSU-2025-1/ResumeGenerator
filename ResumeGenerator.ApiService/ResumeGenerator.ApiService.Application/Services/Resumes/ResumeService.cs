using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Application.Exceptions;
using ResumeGenerator.ApiService.Application.Results;
using ResumeGenerator.ApiService.Data.Context;
using ResumeGenerator.ApiService.Data.Entities;
using ResumeGenerator.Common.Contracts;

namespace ResumeGenerator.ApiService.Application.Services.Resumes;

public sealed class ResumeService : IResumeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IBus _bus;

    public ResumeService(AppDbContext context, IMapper mapper, IBus bus)
    {
        _context = context;
        _mapper = mapper;
        _bus = bus;
    }

    public async Task<Resume> CreateResumeAsync(ResumeDto resume, CancellationToken ct = default)
    {
        var newResume = _mapper.Map<Resume>(resume);
        newResume.UserId = resume.UserId;

        _context.Resumes.Add(newResume);
        await _context.SaveChangesAsync(ct);

        await _bus.Publish(_mapper.Map<CreateResumeCommand>(newResume), ct);

        return newResume;
    }

    public async Task UpdateResumeStatusAsync(Guid resumeId, ResumeStatus newStatus, CancellationToken ct = default)
    {
        var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.Id == resumeId, ct);

        NotFoundException.ThrowIfNull(resume,
            new Error(StatusCodes.Status404NotFound.ToString(), $"Resume with id: {resumeId} not found in database."));

        resume!.ResumeStatus = newStatus;

        await _context.SaveChangesAsync(ct);
    }

    public Task<List<Resume>> GetAllResumesByUserIdAsync(Guid userId, int pageNumber, int pageSize,
        CancellationToken ct = default)
        => _context.Resumes
            .Where(resume => resume.UserId == userId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<Resume> GetResumeByIdAsync(Guid resumeId, Guid userId, CancellationToken ct = default)
    {
        var resume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.Id == resumeId, ct);

        NotFoundException.ThrowIfNull(resume,
            new Error(StatusCodes.Status404NotFound.ToString(), $"Resume with id: {resumeId} not found in database."));

        if (resume!.UserId != userId)
        {
            ForbiddenException.ThrowWithError(new Error(StatusCodes.Status403Forbidden.ToString(),
                $"User with id={userId} can't access this resume."));
        }

        return resume;
    }

    public async Task DeleteResumeByIdAsync(Guid resumeId, Guid userId, CancellationToken ct = default)
    {
        var resume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.Id == resumeId, ct);

        NotFoundException.ThrowIfNull(resume,
            new Error(StatusCodes.Status404NotFound.ToString(), $"Resume with id: {resumeId} not found in database."));

        if (resume!.UserId != userId)
        {
            ForbiddenException.ThrowWithError(new Error(StatusCodes.Status403Forbidden.ToString(),
                $"User with id={userId} can't access this resume."));
        }

        _context.Resumes.Remove(resume);

        await _context.SaveChangesAsync(ct);
    }
}