using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.HasKey(user => user.Id);

            entity.Property(user => user.Name).IsRequired().HasMaxLength(100);

            entity.Property(user => user.Email).IsRequired().HasMaxLength(320);

            entity.Property(user => user.CreatedAt).IsRequired();

            entity.HasOne<Organization>().WithMany().HasForeignKey(user => user.OrganizationId)
                                                    .OnDelete(DeleteBehavior.Restrict);

            // one - to - many relationship
            entity.HasOne<Team>().WithMany().HasForeignKey(user => new { user.OrganizationId, user.TeamId })
                                            .HasPrincipalKey(team => new { team.OrganizationId, team.Id })
                                            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
