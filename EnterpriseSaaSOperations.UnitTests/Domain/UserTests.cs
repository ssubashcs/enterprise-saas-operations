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
    }
}
