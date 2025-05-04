using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.AuthService.Data.Entities
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public long? ChatId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<AuthToken> AuthTokens { get; set; } = new();
        public List<ActivationCode> ActivationCodes { get; set; } = new();
    }
}
