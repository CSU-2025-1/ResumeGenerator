using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ResumeGenerator.ApiService.Application.DTO;
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
        _context.Resumes.Add(newResume);
        await _context.SaveChangesAsync(ct);

        await _bus.Publish(new CreateResumeCommand(
            FirstName: resume.UserFirstName,
            LastName: resume.UserLastName,
            MiddleName: resume.UserPatronymic,
            DesiredPosition: resume.DesiredPosition,
            GitHubLink: resume.GitHubLink,
            TelegramLink: resume.TelegramLink,
            Email: resume.Email,
            PhoneNumber: resume.PhoneNumber,
            Education: resume.Education,
            Experience: resume.ExperienceYears.ToString(),
            HardSkills: resume.HardSkills.Split(", "),
            SoftSkills: resume.HardSkills.Split(", ")
        ), ct);

        return newResume;
    }

    public Task<List<Resume>> GetAllResumesByUserIdAsync(Guid userId, CancellationToken ct = default)
        => _context.Resumes
            .Where(resume => resume.UserId == userId)
            .ToListAsync(ct);
}