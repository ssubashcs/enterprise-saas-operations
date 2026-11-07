using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Infrastructure.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence.Configurations
{
    internal class IdentityUserConfiguration : IEntityTypeConfiguration<ApplicationIdentityUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationIdentityUser> entity)
        {
            entity.Property(user => user.DomainUserId).IsRequired();

            // one-to-one relationship
            entity.HasOne<User>().WithOne().HasForeignKey<ApplicationIdentityUser>(user => user.DomainUserId)
                                           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
