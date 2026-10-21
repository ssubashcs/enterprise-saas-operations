using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public sealed class OperationsDbContext : IdentityUserContext<ApplicationIdentityUser>
    {
        public OperationsDbContext(DbContextOptions<OperationsDbContext> options) : base(options) { }

        public DbSet<Organization> Organizations => Set<Organization>();

        public DbSet<User> DomainUsers => Set<User>();

        public DbSet<Team> Teams => Set<Team>();

        public DbSet<OperationalRequest> OperationalRequests => Set<OperationalRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        
            modelBuilder.Entity<ApplicationIdentityUser>(entity =>
            {
                entity.Property(user => user.DomainUserId).IsRequired();
        
                // one-to-one relationship
                entity.HasOne<User>().WithOne().HasForeignKey<ApplicationIdentityUser>(user => user.DomainUserId)
                                               .OnDelete(DeleteBehavior.Restrict);
            });
        
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(organization => organization.Id);

                entity.Property(organization => organization.Name).IsRequired().HasMaxLength(200);

                entity.Property(organization => organization.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(user => user.Id);

                entity.Property(user => user.Name).IsRequired().HasMaxLength(100);

                entity.Property(user => user.Email).IsRequired().HasMaxLength(320);

                entity.Property(user => user.CreatedAt).IsRequired();

                entity.HasOne<Organization>().WithMany().HasForeignKey(user => user.OrganizationId)
                                                        .OnDelete(DeleteBehavior.Restrict);
                                                                                            
                // one - to - many relationship
                entity.HasOne<Team>().WithMany().HasForeignKey(user => new { user.OrganizationId,  user.TeamId })
                                                .HasPrincipalKey(team => new { team.OrganizationId, team.Id })
                                                .OnDelete(DeleteBehavior.Restrict);
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

                entity.HasOne<User>().WithMany().HasForeignKey(request => new { request.OrganizationId, request.RequesterUserId })
                                                .HasPrincipalKey(user => new {user.OrganizationId, user.Id })
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
            });
        }
    }
}
