using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Configurations
{
    internal class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> entity)
        {
            entity.HasKey(organization => organization.Id);

            entity.Property(organization => organization.Name).IsRequired().HasMaxLength(200);

            entity.Property(organization => organization.CreatedAt).IsRequired();
        }
    }
}
