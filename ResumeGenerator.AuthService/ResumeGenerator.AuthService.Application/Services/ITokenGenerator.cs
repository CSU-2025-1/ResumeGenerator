using System.Security.Claims;
using ResumeGenerator.AuthService.Data.Entities;

namespace ResumeGenerator.AuthService.Application.Services;

public interface ITokenGenerator
{
    string GenerateToken(User user);
    ClaimsPrincipal ValidateToken(string token);
}

