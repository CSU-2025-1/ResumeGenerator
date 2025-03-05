using ResumeGenerator.ApiService.Application.DTO;
using ResumeGenerator.ApiService.Data.Entities;

namespace ResumeGenerator.ApiService.Application.Mapping;

public class AppMappingProfile : MappingProfileBase
{
    public AppMappingProfile()
    {
        CreateMap<Resume, ResumeDto>();
    }
}