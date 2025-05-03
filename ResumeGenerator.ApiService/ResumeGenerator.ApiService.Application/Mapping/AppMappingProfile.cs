using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Entities;
using ResumeGenerator.Common.Contracts;

namespace ResumeGenerator.ApiService.Application.Mapping;

public sealed class AppMappingProfile : MappingProfileBase
{
    public AppMappingProfile()
    {
        CreateMap<Resume, ResumeDto>()
            .ForMember(dest => dest.HardSkills,
                opt => opt.MapFrom(src => string.Join(", ", src.HardSkills.Select(h => h.HardSkillName))))
            .ForMember(dest => dest.SoftSkills,
                opt => opt.MapFrom(src => string.Join(", ", src.SoftSkills.Select(s => s.SoftSkillName))));

        CreateMap<ResumeDto, Resume>()
            .ForMember(dest => dest.ResumeStatus, opt => opt.Ignore())
            .ForMember(dest => dest.HardSkills, opt => opt.MapFrom(src => src.HardSkills
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(name => new HardSkill { HardSkillName = name }).ToList()))
            .ForMember(dest => dest.SoftSkills, opt => opt.MapFrom(src => src.SoftSkills
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(name => new SoftSkill { SoftSkillName = name }).ToList()));

        CreateMap<Resume, CreateResumeCommand>()
            .ForMember(dest => dest.HardSkills,
                opt => opt.MapFrom(src => src.HardSkills.Select(h => h.HardSkillName).ToArray()))
            .ForMember(dest => dest.SoftSkills,
                opt => opt.MapFrom(src => src.SoftSkills.Select(s => s.SoftSkillName).ToArray()))
            .ForMember(dest => dest.ResumeId,
                opt => opt.MapFrom(src => src.Id));
    }
}