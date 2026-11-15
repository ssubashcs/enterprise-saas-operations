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

        private static OperationalRequest CreateRequest()
        {
            return new(organizationId: 1, requesterUserId: 10, title: "Fix login issue",
                       description: "Users cannot log in.", priority: RequestPriority.High);
        }
    }
}
