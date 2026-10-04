using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public sealed class EnterpriseSaaSOperationsDbContext(DbContextOptions<EnterpriseSaaSOperationsDbContext> options) : DbContext(options)
    {
        public DbSet<Organization> Organizations => Set<Organization>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(organization => organization.Id);

                entity.Property(organization => organization.Name).IsRequired().HasMaxLength(200);

                entity.Property(organization => organization.CreatedAt).IsRequired();
            });
        }
    }
}
