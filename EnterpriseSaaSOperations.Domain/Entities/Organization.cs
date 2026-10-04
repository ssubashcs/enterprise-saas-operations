namespace EnterpriseSaaSOperations.Domain.Entities
{
    public sealed class Organization
    {
        public Organization(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public int Id { get; private set; }

        public string Name { get; private set; } 

        public DateTimeOffset CreatedAt { get; private set; }

        public void Rename(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
        }
    }
}
