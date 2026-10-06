using EnterpriseSaaSOperations.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public sealed class EnterpriseSaaSOperationsDbContext(DbContextOptions<EnterpriseSaaSOperationsDbContext> options) : DbContext(options)
    {
        public DbSet<Organization> Organizations => Set<Organization>();

        public DbSet<User> Users => Set<User>();

        public DbSet<Team> Teams => Set<Team>();

        public DbSet<OperationalRequest> OperationalRequests => Set<OperationalRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(organization => organization.Id);

                entity.Property(organization => organization.Name).IsRequired().HasMaxLength(200);

                entity.Property(organization => organization.CreatedAt).IsRequired();

                // entity.HasMany<User>().WithOne().HasForeignKey(user => user.OrganizationId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);

                entity.Property(user => user.Name).IsRequired().HasMaxLength(100);

                entity.Property(user => user.Email).IsRequired().HasMaxLength(320);

                entity.Property(user => user.CreatedAt).IsRequired();

                entity.HasOne<Organization>().WithMany().HasForeignKey(user => user.OrganizationId)
                                                        .OnDelete(DeleteBehavior.Restrict);

                // one - to - many relationship with an optional foreign key
                entity.HasOne<Team>().WithMany().HasForeignKey(user => user.TeamId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(team => team.Id);

                entity.Property(team => team.Name).IsRequired().HasMaxLength(100);

                entity.Property(team => team.CreatedAt).IsRequired();

                entity.HasOne<Organization>().WithMany().HasForeignKey(team => team.OrganizationId)
                                                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OperationalRequest>(entity =>
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

                entity.HasOne<User>().WithMany().HasForeignKey(request => request.RequesterUserId)
                                                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Team>().WithMany().HasForeignKey(request => request.TeamId)
                                                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>().WithMany().HasForeignKey(request => request.AssigneeUserId)
                                                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
