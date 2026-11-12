using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Domain.Enums;

namespace EnterpriseSaaSOperations.UnitTests.Domain
{
    public class UserTests
    {
        [Fact]
        public void ChangeRole_ShouldUpdateRole_WhenNewRoleIsValid()
        {
            // Arrange
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com");

            // Act
            user.ChangeRole(UserRole.Administrator);

            // Assert
            Assert.Equal(UserRole.Administrator, user.Role);
        }

        [Fact]
        public void ChangeRole_ShouldThrow_WhenUserIsOwner()
        {
            // Arrange
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com", role: UserRole.Owner);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => user.ChangeRole(UserRole.Administrator));

            Assert.Equal(UserRole.Owner, user.Role);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(999)]
        public void ChangeRole_ShouldThrow_WhenRoleIsUndefined(int invalidRole)
        {
            // Arrange
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com");

            UserRole role = (UserRole)invalidRole;

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => user.ChangeRole(role));

            Assert.Equal(nameof(role), exception.ParamName);
            Assert.Equal(UserRole.Member, user.Role);
        }

        [Fact]
        public void ChangeRole_ShouldThrow_WhenNewRoleIsOwner()
        {
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com");

            var exception = Assert.Throws<InvalidOperationException>(() => user.ChangeRole(UserRole.Owner));

            Assert.Equal(UserRole.Member, user.Role);
        }

        [Fact]
        public void ChangeRole_ShouldAllow_WhenNewRoleMatchesCurrentRole()
        {
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com", role: UserRole.Administrator);

            user.ChangeRole(UserRole.Administrator);

            Assert.Equal(UserRole.Administrator, user.Role);
        }

        [Fact]
        public void Constructor_ShouldTrimNameAndEmail()
        {
            User user = new(organizationId: 1, name: "  Ben  ", email: "  ben@example.com  ");

            Assert.Equal("Ben", user.Name);
            Assert.Equal("ben@example.com", user.Email);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNameIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new User(organizationId: 1, 
                                                                                name: null!, 
                                                                                email: "ben@example.com"));

            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_ShouldThrow_WhenNameIsEmptyOrWhitespace(string invalidName)
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(organizationId: 1,
                                                                            name: invalidName,
                                                                            email: "ben@example.com"));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenEmailIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new User(organizationId: 1,
                                                                                name: "Ben",
                                                                                email: null!));

            Assert.Equal("email", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_ShouldThrow_WhenEmailIsEmptyOrWhitespace(string invalidEmail)
        {
            var exception = Assert.Throws<ArgumentException>(() => new User(organizationId: 1,
                                                                            name: "Ben",
                                                                            email: invalidEmail));

            Assert.Equal("email", exception.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(999)]
        public void Constructor_ShouldThrow_WhenRoleIsUndefined(int invalidRole)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new User(organizationId: 1,
                                                                                      name: "Ben",
                                                                                      email: "ben@example.com",
                                                                                      role: (UserRole)invalidRole));

            Assert.Equal("role", exception.ParamName);
        }

        [Fact]
        public void AssignToTeam_ShouldSetTeamId_WhenTeamIdIsPositive()
        {
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com");

            user.AssignToTeam(10);

            Assert.Equal(10, user.TeamId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void AssignToTeam_ShouldThrow_WhenTeamIdIsNotPositive(int invalidTeamId)
        {
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com");

            user.AssignToTeam(10);

            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => user.AssignToTeam(invalidTeamId));

            Assert.Equal("teamId", exception.ParamName);
            Assert.Equal(10, user.TeamId);
        }

        [Fact]
        public void RemoveFromTeam_ShouldClearTeamId_WhenUserBelongsToTeam()
        {
            User user = new(organizationId: 1, name: "Ben", email: "ben@example.com");

            user.AssignToTeam(10);
            user.RemoveFromTeam();

            Assert.Null(user.TeamId);
        }
    }
}
