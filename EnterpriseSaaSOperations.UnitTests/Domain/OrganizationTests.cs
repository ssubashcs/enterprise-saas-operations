using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.UnitTests.Domain
{
    public class OrganizationTests
    {
        [Fact]
        public void Constructor_ShouldTrimName()
        {
            Organization organization = new(name: "  ABC Corporation  ");

            Assert.Equal("ABC Corporation", organization.Name);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNameIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Organization(name: null!));

            Assert.Equal("name", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Constructor_ShouldThrow_WhenNameIsEmptyOrWhitespace(string name)
        {
            var exception = Assert.Throws<ArgumentException>(() => new Organization(name: name));

            Assert.Equal("name", exception.ParamName);
        }

        [Fact]
        public void Constructor_ShouldInitializeCreatedAtInUtc()
        {
            DateTimeOffset beforeCreation = DateTimeOffset.UtcNow;

            Organization organization = new(name: "ABC Corporation");

            DateTimeOffset afterCreation = DateTimeOffset.UtcNow;

            Assert.InRange(organization.CreatedAt, beforeCreation, afterCreation);
            Assert.Equal(TimeSpan.Zero, organization.CreatedAt.Offset);
        }

        [Fact]
        public void Rename_ShouldUpdateNameAndTrimWhitespace()
        {
            Organization organization = new(name: "ABC Corporation");

            organization.Rename("  Globex Corporation  ");

            Assert.Equal("Globex Corporation", organization.Name);
        }

        [Fact]
        public void Rename_ShouldThrow_WhenNameIsNull()
        {
            Organization organization = new(name: "ABC Corporation");

            var exception = Assert.Throws<ArgumentNullException>(() => organization.Rename(null!));

            Assert.Equal("name", exception.ParamName);
            Assert.Equal("ABC Corporation", organization.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        public void Rename_ShouldThrow_WhenNameIsEmptyOrWhitespace(string name)
        {
            Organization organization = new(name: "ABC Corporation");

            var exception = Assert.Throws<ArgumentException>(() => organization.Rename(name));

            Assert.Equal("name", exception.ParamName);
            Assert.Equal("ABC Corporation", organization.Name);
        }
    }
}
