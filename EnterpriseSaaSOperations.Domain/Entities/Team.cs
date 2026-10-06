namespace EnterpriseSaaSOperations.Domain.Entities
{
    public class Team
    {
        public Team(int organizationId, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            OrganizationId = organizationId;
            Name = name.Trim();
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public int Id { get; private set; }

        public int OrganizationId { get; private set; }

        public string Name { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public void Rename(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
        }
    }
}
