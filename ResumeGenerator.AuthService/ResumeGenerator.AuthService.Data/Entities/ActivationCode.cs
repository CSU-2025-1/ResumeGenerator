using System.ComponentModel.DataAnnotations;

namespace ResumeGenerator.AuthService.Data.Entities
{
    public sealed class ActivationCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Code { get; set; } 
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
    }
}
