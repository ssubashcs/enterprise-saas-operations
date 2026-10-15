using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.Domain.Entities
{
    public sealed class OperationalRequest
    {
        public OperationalRequest(int organizationId, int requesterUserId, string title, 
                                  string description, RequestPriority priority)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);

            if (!Enum.IsDefined(priority)) throw new ArgumentOutOfRangeException(nameof(priority));

            OrganizationId = organizationId;
            RequesterUserId = requesterUserId;
            Title = title.Trim();
            Description = description.Trim();
            Priority = priority;
            Status = RequestStatus.Created;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public int Id { get; private set; }

        public int OrganizationId { get; private set; }

        public int RequesterUserId { get; private set; }

        public int? TeamId { get; private set; }

        public int? AssigneeUserId { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public RequestStatus Status { get; private set; }

        public RequestPriority Priority { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        public void Assign(int teamId, int assigneeUserId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(teamId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(assigneeUserId);

            if (Status is not (RequestStatus.Created or RequestStatus.Assigned))
            {
                throw new InvalidOperationException($"Cannot assign a request while its status is {Status}.");
            }

            TeamId = teamId;
            AssigneeUserId = assigneeUserId;
            Status = RequestStatus.Assigned; 
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void ChangeStatus(RequestStatus newStatus)
        {
            if (!Enum.IsDefined(newStatus))
            {
                throw new ArgumentOutOfRangeException(nameof(newStatus));
            }

            if (!IsValidTransition(Status, newStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot transition request from {Status} to {newStatus}.");
            }

            Status = newStatus;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        private static bool IsValidTransition(
            RequestStatus currentStatus,
            RequestStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (RequestStatus.Created, RequestStatus.Assigned) => true,
                (RequestStatus.Assigned, RequestStatus.InProgress) => true,
                (RequestStatus.InProgress, RequestStatus.Resolved) => true,
                (RequestStatus.Resolved, RequestStatus.Closed) => true,
                _ => false
            };
        }
    }
}
