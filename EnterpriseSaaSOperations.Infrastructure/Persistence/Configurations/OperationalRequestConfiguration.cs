using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Configurations
{
    internal class OperationalRequestConfiguration : IEntityTypeConfiguration<OperationalRequest>
    {
        public void Configure(EntityTypeBuilder<OperationalRequest> entity)
        {
            entity.HasKey(request => request.Id);

            entity.Property(request => request.Title).IsRequired().HasMaxLength(200);

            entity.Property(request => request.Description).IsRequired().HasMaxLength(2000);

            entity.Property(request => request.Status).IsRequired();

            entity.Property(request => request.Priority).IsRequired();

            entity.Property(request => request.CreatedAt).IsRequired();

            entity.Property(request => request.UpdatedAt).IsRequired();

            entity.HasOne<Organization>().WithMany().HasForeignKey(operation => operation.OrganizationId)
                                                    .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>().WithMany().HasForeignKey(request => new { request.OrganizationId, request.RequesterUserId })
                                            .HasPrincipalKey(user => new { user.OrganizationId, user.Id })
                                            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Team>().WithMany().HasForeignKey(request => new { request.OrganizationId, request.TeamId })
                                            .HasPrincipalKey(team => new { team.OrganizationId, team.Id })
                                            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>().WithMany().HasForeignKey(request => new { request.OrganizationId, request.AssigneeUserId })
                                            .HasPrincipalKey(user => new { user.OrganizationId, user.Id })
                                            .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes
            entity.HasIndex(request => new { request.OrganizationId, request.Status });

            entity.HasIndex(request => new { request.OrganizationId, request.AssigneeUserId });

            entity.HasIndex(request => new { request.OrganizationId, request.TeamId });
        }   
    }
}
