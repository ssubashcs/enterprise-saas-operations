using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Domain.Entities
{
    public sealed class User
    {
        public User(int organizationId, string name, string email, UserRole role = UserRole.Member)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(email);

            OrganizationId = organizationId;
            Name = name.Trim();
            Email = email.Trim();
            Role = role;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public int Id { get; private set; }

        public int OrganizationId { get; private set; }

        public int? TeamId { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public UserRole Role { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public void ChangeRole(UserRole role)
        {
            Role = role;
        }

        public void AssignToTeam(int teamId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(teamId);

            TeamId = teamId;
        }
    }
}
