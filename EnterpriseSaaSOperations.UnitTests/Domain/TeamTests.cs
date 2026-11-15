using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.UnitTests.Domain
{
    public class TeamTests
    {
        [Fact]
        public void Constructor_ShouldTrimName()
        {
            Team team = new(organizationId: 1, name: "  Engineering  ");

            Assert.Equal("Engineering", team.Name);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNameIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Team(organizationId: 1, name: null!));

            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Constructor_ShouldThrow_WhenNameIsEmptyOrWhitespace(string name)
        {
            var exception = Assert.Throws<ArgumentException>(() => new Team(organizationId: 1, name: name));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void Rename_ShouldUpdateNameAndTrimWhitespace()
        {
            Team team = new(organizationId: 1, name: "Engineering");

            team.Rename("  Development  ");

            Assert.Equal("Development", team.Name);
        }

        [Fact]
        public void Rename_ShouldThrow_WhenNameIsNull()
        {
            Team team = new(organizationId: 1, name: "Engineering");

            var exception = Assert.Throws<ArgumentNullException>(() => team.Rename(null!));

            Assert.Equal("name", exception.ParamName);
            Assert.Equal("Engineering", team.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("\t")]
        public void Rename_ShouldThrow_WhenNameIsEmptyOrWhitespace(string name)
        {
            Team team = new(organizationId: 1, name: "Engineering");

            var exception = Assert.Throws<ArgumentException>(() => team.Rename(name));

            Assert.Equal("name", exception.ParamName);
            Assert.Equal("Engineering", team.Name);
        }

        [Fact]
        public void Constructor_ShouldInitializeOrganizationIdAndCreatedAt()
        {
            DateTimeOffset beforeCreation = DateTimeOffset.UtcNow;

            Team team = new(organizationId: 1, name: "Engineering");

            DateTimeOffset afterCreation = DateTimeOffset.UtcNow;

            Assert.Equal(1, team.OrganizationId);
            Assert.InRange(team.CreatedAt, beforeCreation, afterCreation);
            Assert.Equal(TimeSpan.Zero, team.CreatedAt.Offset);
        }
    }
}
