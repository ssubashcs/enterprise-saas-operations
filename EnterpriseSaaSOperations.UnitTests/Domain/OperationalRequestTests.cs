using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.UnitTests.Domain
{
    public class OperationalRequestTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesAndTrimText()
        {
            OperationalRequest request = new(organizationId: 1, requesterUserId: 10, title: "  Fix login issue  ",
                                             description: "  Users cannot log in.  ", priority: RequestPriority.High);

            Assert.Equal(1, request.OrganizationId);
            Assert.Equal(10, request.RequesterUserId);
            Assert.Equal("Fix login issue", request.Title);
            Assert.Equal("Users cannot log in.", request.Description);
            Assert.Equal(RequestPriority.High, request.Priority);
            Assert.Equal(RequestStatus.Created, request.Status);
            Assert.Null(request.TeamId);
            Assert.Null(request.AssigneeUserId);
            Assert.Equal(request.CreatedAt, request.UpdatedAt);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenTitleIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OperationalRequest(organizationId: 1,
                                                                                              requesterUserId: 10,
                                                                                              title: null!,
                                                                                              description: "Users cannot log in.",
                                                                                              priority: RequestPriority.High));

            Assert.Equal("title", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_ShouldThrow_WhenTitleIsEmptyOrWhitespace(string title)
        {
            var exception = Assert.Throws<ArgumentException>(() => new OperationalRequest(organizationId: 1,
                                                                                          requesterUserId: 10,
                                                                                          title: title,
                                                                                          description: "Users cannot log in.",
                                                                                          priority: RequestPriority.High));

            Assert.Equal("title", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenDescriptionIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new OperationalRequest(organizationId: 1,
                                                                                              requesterUserId: 10,
                                                                                              title: "Fix login issue",
                                                                                              description: null!,
                                                                                              priority: RequestPriority.High));

            Assert.Equal("description", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_ShouldThrow_WhenDescriptionIsEmptyOrWhitespace(string description)
        {
            var exception = Assert.Throws<ArgumentException>(() => new OperationalRequest(organizationId: 1,
                                                                                          requesterUserId: 10,
                                                                                          title: "Fix login issue",
                                                                                          description: description,
                                                                                          priority: RequestPriority.High));

            Assert.Equal("description", exception.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(999)]
        public void Constructor_ShouldThrow_WhenPriorityIsUndefined(int invalidPriority)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new OperationalRequest(organizationId: 1,
                                                                                                    requesterUserId: 10,
                                                                                                    title: "Fix login issue",
                                                                                                    description: "Users cannot log in.",
                                                                                                    priority: (RequestPriority)invalidPriority));

            Assert.Equal("priority", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldInitializeTimestampsInUtc()
        {
            DateTimeOffset beforeCreation = DateTimeOffset.UtcNow;

            OperationalRequest request = CreateRequest();

            DateTimeOffset afterCreation = DateTimeOffset.UtcNow;

            Assert.InRange(request.CreatedAt, beforeCreation, afterCreation);
            Assert.Equal(TimeSpan.Zero, request.CreatedAt.Offset);
            Assert.Equal(request.CreatedAt, request.UpdatedAt);
            Assert.Equal(TimeSpan.Zero, request.UpdatedAt.Offset);
        }

        [Fact]
        public void Assign_ShouldSetAssignmentDetailsAndStatus_WhenRequestIsCreated()
        {
            OperationalRequest request = CreateRequest();

            DateTimeOffset beforeAssignment = DateTimeOffset.UtcNow;

            request.Assign(teamId: 5, assigneeUserId: 20);

            DateTimeOffset afterAssignment = DateTimeOffset.UtcNow;

            Assert.Equal(5, request.TeamId);
            Assert.Equal(20, request.AssigneeUserId);
            Assert.Equal(RequestStatus.Assigned, request.Status);
            Assert.InRange(request.UpdatedAt, beforeAssignment, afterAssignment);
            Assert.Equal(TimeSpan.Zero, request.UpdatedAt.Offset);
        }

        [Fact]
        public void Assign_ShouldAllowReassignment_WhenRequestIsAlreadyAssigned()
        {
            OperationalRequest request = CreateRequest();

            request.Assign(teamId: 5, assigneeUserId: 20);

            DateTimeOffset beforeReassignment = DateTimeOffset.UtcNow;

            request.Assign(teamId: 6, assigneeUserId: 30);

            DateTimeOffset afterReassignment = DateTimeOffset.UtcNow;

            Assert.Equal(6, request.TeamId);
            Assert.Equal(30, request.AssigneeUserId);
            Assert.Equal(RequestStatus.Assigned, request.Status);
            Assert.InRange(request.UpdatedAt, beforeReassignment, afterReassignment);
            Assert.Equal(TimeSpan.Zero, request.UpdatedAt.Offset);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Assign_ShouldThrow_WhenTeamIdIsNotPositive(int teamId)
        {
            OperationalRequest request = CreateRequest();

            request.Assign(teamId: 5, assigneeUserId: 20);
            DateTimeOffset originalUpdatedAt = request.UpdatedAt;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => request.Assign(teamId: teamId, assigneeUserId: 30));

            Assert.Equal("teamId", exception.ParamName);
            Assert.Equal(5, request.TeamId);
            Assert.Equal(20, request.AssigneeUserId);
            Assert.Equal(RequestStatus.Assigned, request.Status);
            Assert.Equal(originalUpdatedAt, request.UpdatedAt);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Assign_ShouldThrow_WhenAssigneeUserIdIsNotPositive(int assigneeUserId)
        {
            OperationalRequest request = CreateRequest();

            request.Assign(teamId: 5, assigneeUserId: 20);
            DateTimeOffset originalUpdatedAt = request.UpdatedAt;

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => request.Assign(teamId: 6, assigneeUserId: assigneeUserId));

            Assert.Equal("assigneeUserId", exception.ParamName);
            Assert.Equal(5, request.TeamId);
            Assert.Equal(20, request.AssigneeUserId);
            Assert.Equal(RequestStatus.Assigned, request.Status);
            Assert.Equal(originalUpdatedAt, request.UpdatedAt);
        }

        [Theory]
        [InlineData(RequestStatus.InProgress)]
        [InlineData(RequestStatus.Resolved)]
        [InlineData(RequestStatus.Closed)]
        public void Assign_ShouldThrow_WhenRequestIsNotCreatedOrAssigned(RequestStatus status)
        {
            OperationalRequest request = CreateRequest();

            request.Assign(teamId: 5, assigneeUserId: 20);

            switch (status)
            {
                case RequestStatus.InProgress:
                    request.ChangeStatus(RequestStatus.InProgress);
                    break;

                case RequestStatus.Resolved:
                    request.ChangeStatus(RequestStatus.InProgress);
                    request.ChangeStatus(RequestStatus.Resolved);
                    break;

                case RequestStatus.Closed:
                    request.ChangeStatus(RequestStatus.InProgress);
                    request.ChangeStatus(RequestStatus.Resolved);
                    request.ChangeStatus(RequestStatus.Closed);
                    break;
            }

            int? originalTeamId = request.TeamId;
            int? originalAssigneeUserId = request.AssigneeUserId;
            DateTimeOffset originalUpdatedAt = request.UpdatedAt;

            var exception = Assert.Throws<InvalidOperationException>(() => request.Assign(teamId: 6, assigneeUserId: 30));

            Assert.Contains($"Cannot assign a request while its status is {status}.", exception.Message);
            Assert.Equal(status, request.Status);
            Assert.Equal(originalTeamId, request.TeamId);
            Assert.Equal(originalAssigneeUserId, request.AssigneeUserId);
            Assert.Equal(originalUpdatedAt, request.UpdatedAt);
        }

        [Fact]
        public void ChangeStatus_ShouldMoveToInProgress_WhenRequestIsAssigned()
        {
            OperationalRequest request = CreateAssignedRequest();

            DateTimeOffset beforeChange = DateTimeOffset.UtcNow;

            request.ChangeStatus(RequestStatus.InProgress);

            DateTimeOffset afterChange = DateTimeOffset.UtcNow;

            Assert.Equal(RequestStatus.InProgress, request.Status);
            Assert.InRange(request.UpdatedAt, beforeChange, afterChange);
            Assert.Equal(TimeSpan.Zero, request.UpdatedAt.Offset);
        }

        [Fact]
        public void ChangeStatus_ShouldMoveToResolved_WhenRequestIsInProgress()
        {
            OperationalRequest request = CreateAssignedRequest();
            request.ChangeStatus(RequestStatus.InProgress);

            DateTimeOffset beforeChange = DateTimeOffset.UtcNow;

            request.ChangeStatus(RequestStatus.Resolved);

            DateTimeOffset afterChange = DateTimeOffset.UtcNow;

            Assert.Equal(RequestStatus.Resolved, request.Status);
            Assert.InRange(request.UpdatedAt, beforeChange, afterChange);
            Assert.Equal(TimeSpan.Zero, request.UpdatedAt.Offset);
        }

        [Fact]
        public void ChangeStatus_ShouldMoveToClosed_WhenRequestIsResolved()
        {
            OperationalRequest request = CreateAssignedRequest();
            request.ChangeStatus(RequestStatus.InProgress);
            request.ChangeStatus(RequestStatus.Resolved);

            DateTimeOffset beforeChange = DateTimeOffset.UtcNow;

            request.ChangeStatus(RequestStatus.Closed);

            DateTimeOffset afterChange = DateTimeOffset.UtcNow;

            Assert.Equal(RequestStatus.Closed, request.Status);
            Assert.InRange(request.UpdatedAt, beforeChange, afterChange);
            Assert.Equal(TimeSpan.Zero, request.UpdatedAt.Offset);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(999)]
        public void ChangeStatus_ShouldThrow_WhenStatusIsUndefined(int invalidStatus)
        {
            OperationalRequest request = CreateRequest();

            DateTimeOffset originalUpdatedAt = request.UpdatedAt;

            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => request.ChangeStatus((RequestStatus)invalidStatus));

            Assert.Equal("newStatus", exception.ParamName);
            Assert.Equal(RequestStatus.Created, request.Status);
            Assert.Equal(originalUpdatedAt, request.UpdatedAt);
        }

        [Theory]
        [InlineData(RequestStatus.Created, RequestStatus.Created)]
        [InlineData(RequestStatus.Created, RequestStatus.Assigned)]
        [InlineData(RequestStatus.Created, RequestStatus.InProgress)]
        [InlineData(RequestStatus.Created, RequestStatus.Resolved)]
        [InlineData(RequestStatus.Created, RequestStatus.Closed)]

        [InlineData(RequestStatus.Assigned, RequestStatus.Created)]
        [InlineData(RequestStatus.Assigned, RequestStatus.Assigned)]
        [InlineData(RequestStatus.Assigned, RequestStatus.Resolved)]
        [InlineData(RequestStatus.Assigned, RequestStatus.Closed)]

        [InlineData(RequestStatus.InProgress, RequestStatus.Created)]
        [InlineData(RequestStatus.InProgress, RequestStatus.Assigned)]
        [InlineData(RequestStatus.InProgress, RequestStatus.InProgress)]
        [InlineData(RequestStatus.InProgress, RequestStatus.Closed)]

        [InlineData(RequestStatus.Resolved, RequestStatus.Created)]
        [InlineData(RequestStatus.Resolved, RequestStatus.Assigned)]
        [InlineData(RequestStatus.Resolved, RequestStatus.InProgress)]
        [InlineData(RequestStatus.Resolved, RequestStatus.Resolved)]

        [InlineData(RequestStatus.Closed, RequestStatus.Created)]
        [InlineData(RequestStatus.Closed, RequestStatus.Assigned)]
        [InlineData(RequestStatus.Closed, RequestStatus.InProgress)]
        [InlineData(RequestStatus.Closed, RequestStatus.Resolved)]
        [InlineData(RequestStatus.Closed, RequestStatus.Closed)]
        public void ChangeStatus_ShouldThrow_WhenTransitionIsInvalid(RequestStatus currentStatus, RequestStatus newStatus)
        {
            OperationalRequest request = CreateRequestInStatus(currentStatus);

            DateTimeOffset originalUpdatedAt = request.UpdatedAt;
            int? originalTeamId = request.TeamId;
            int? originalAssigneeUserId = request.AssigneeUserId;

            var exception = Assert.Throws<InvalidOperationException>(() => request.ChangeStatus(newStatus));

            Assert.Contains($"Cannot transition request from {currentStatus} to {newStatus}.", exception.Message);

            Assert.Equal(currentStatus, request.Status);
            Assert.Equal(originalTeamId, request.TeamId);
            Assert.Equal(originalAssigneeUserId, request.AssigneeUserId);
            Assert.Equal(originalUpdatedAt, request.UpdatedAt);
        }

        private static OperationalRequest CreateRequest()
        {
            return new(organizationId: 1, requesterUserId: 10, title: "Fix login issue",
                       description: "Users cannot log in.", priority: RequestPriority.High);
        }

        private static OperationalRequest CreateAssignedRequest()
        {
            OperationalRequest request = CreateRequest();
            
            request.Assign(teamId: 5, assigneeUserId: 20);

            return request;
        }

        private static OperationalRequest CreateRequestInStatus(RequestStatus status)
        {
            OperationalRequest request = CreateRequest();

            switch (status)
            {
                case RequestStatus.Created:
                    return request;

                case RequestStatus.Assigned:
                    request.Assign(teamId: 5, assigneeUserId: 20);
                    return request;

                case RequestStatus.InProgress:
                    request.Assign(teamId: 5, assigneeUserId: 20);
                    request.ChangeStatus(RequestStatus.InProgress);
                    return request;

                case RequestStatus.Resolved:
                    request.Assign(teamId: 5, assigneeUserId: 20);
                    request.ChangeStatus(RequestStatus.InProgress);
                    request.ChangeStatus(RequestStatus.Resolved);
                    return request;

                case RequestStatus.Closed:
                    request.Assign(teamId: 5, assigneeUserId: 20);
                    request.ChangeStatus(RequestStatus.InProgress);
                    request.ChangeStatus(RequestStatus.Resolved);
                    request.ChangeStatus(RequestStatus.Closed);
                    return request;

                default:
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
        }
    }
}
