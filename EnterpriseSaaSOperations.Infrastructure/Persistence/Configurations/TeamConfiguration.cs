using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Configurations
{
    internal sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> entity)
        {
            entity.HasKey(team => team.Id);

            entity.Property(team => team.Name).IsRequired().HasMaxLength(100);

            entity.Property(team => team.CreatedAt).IsRequired();

            entity.HasOne<Organization>().WithMany().HasForeignKey(team => team.OrganizationId)
                                                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
