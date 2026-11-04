using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using EnterpriseSaaSOperations.Application.Auditing;
using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Infrastructure.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public sealed class OperationsDbContext : IdentityUserContext<ApplicationIdentityUser>
    {
        public OperationsDbContext(DbContextOptions<OperationsDbContext> options) : base(options) { }

        public DbSet<Organization> Organizations => Set<Organization>();

        public DbSet<User> DomainUsers => Set<User>();

        public DbSet<Team> Teams => Set<Team>();

        public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

        public DbSet<OperationalRequest> OperationalRequests => Set<OperationalRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OperationsDbContext).Assembly);
        }
    }
}
