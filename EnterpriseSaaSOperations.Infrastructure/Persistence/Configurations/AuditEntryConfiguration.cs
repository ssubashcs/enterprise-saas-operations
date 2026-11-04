using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EnterpriseSaaSOperations.Application.Auditing;
using EnterpriseSaaSOperations.Domain.Entities;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Configurations
{
    internal sealed class AuditEntryConfiguration : IEntityTypeConfiguration<AuditEntry>
    {
        public void Configure(EntityTypeBuilder<AuditEntry> entity)
        {
            entity.HasKey(audit => audit.Id);

            entity.Property(audit => audit.Action).HasConversion<string>().HasMaxLength(100).IsRequired();

            entity.Property(audit => audit.TargetType).HasConversion<string>().HasMaxLength(100).IsRequired();

            entity.Property(audit => audit.OccurredAtUtc).IsRequired();

            entity.Property(audit => audit.DetailsJson).HasMaxLength(4000);

            entity.HasOne<Organization>().WithMany().HasForeignKey(audit => audit.OrganizationId)
                                                    .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>().WithMany().HasForeignKey(audit => new { audit.OrganizationId, audit.ActorUserId })
                                            .HasPrincipalKey(user => new { user.OrganizationId, user.Id })
                                            .OnDelete(DeleteBehavior.Restrict);

            // supports querying an organization's audit history chronologically.
            entity.HasIndex(audit => new { audit.OrganizationId, audit.OccurredAtUtc });

            entity.HasIndex(audit => new { audit.OrganizationId, audit.TargetType, audit.TargetId });
        }
    }
}
