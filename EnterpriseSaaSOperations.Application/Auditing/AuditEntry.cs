namespace EnterpriseSaaSOperations.Application.Auditing
{
    public sealed class AuditEntry
    {
        public AuditEntry(int organizationId, int actorUserId, AuditAction action, AuditTargetType targetType,
                          int targetId, DateTimeOffset occurredAtUtc, string? detailsJson = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(organizationId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(actorUserId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(targetId);

            if (!Enum.IsDefined(action)) throw new ArgumentOutOfRangeException(nameof(action));

            if (!Enum.IsDefined(targetType)) throw new ArgumentOutOfRangeException(nameof(targetType));

            if (occurredAtUtc == default)
            {
                throw new ArgumentException("The occurrence timestamp must be specified.", nameof(occurredAtUtc));
            }

            OrganizationId = organizationId;
            ActorUserId = actorUserId;
            Action = action;
            TargetType = targetType;
            TargetId = targetId;
            OccurredAtUtc = occurredAtUtc;
            DetailsJson = detailsJson;
        }

        public int Id { get; private set; }

        public int OrganizationId { get; private set; }

        public int ActorUserId { get; private set; }

        public AuditAction Action { get; private set; }

        public AuditTargetType TargetType { get; private set; }

        public int TargetId { get; private set; }

        public DateTimeOffset OccurredAtUtc { get; private set; }

        public string? DetailsJson { get; private set; }
    }
}
