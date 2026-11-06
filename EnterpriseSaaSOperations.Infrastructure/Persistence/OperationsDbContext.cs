using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using EnterpriseSaaSOperations.Domain.Entities;
using EnterpriseSaaSOperations.Application.Auditing;
using EnterpriseSaaSOperations.Infrastructure.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Persistence
{
    public sealed class OperationsDbContext : IdentityUserContext<ApplicationIdentityUser>
    {
        public OperationsDbContext(DbContextOptions<OperationsDbContext> options) : base(options) { }

        internal DbSet<Organization> Organizations => Set<Organization>();

        internal DbSet<User> DomainUsers => Set<User>();

        internal DbSet<Team> Teams => Set<Team>();

        internal DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

        internal DbSet<OperationalRequest> OperationalRequests => Set<OperationalRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OperationsDbContext).Assembly);
        }
    }
}
