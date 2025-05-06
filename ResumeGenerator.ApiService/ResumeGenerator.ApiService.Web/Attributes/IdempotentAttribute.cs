using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using ResumeGenerator.ApiService.Web.ActionFilters;

namespace ResumeGenerator.ApiService.Web.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class IdempotentAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        => new IdempotencyActionFilter(
            cache: serviceProvider.GetRequiredService<IDistributedCache>(),
            logger: serviceProvider.GetRequiredService<ILogger<IdempotencyActionFilter>>()
        );
}