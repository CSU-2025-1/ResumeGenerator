using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Mapping;

public sealed class AppMappingProfile : MappingProfileBase
{
    public AppMappingProfile()
    {
        CreateMap<Resume, ResumeDto>();
        CreateMap<ResumeDto, Resume>();
    }
}