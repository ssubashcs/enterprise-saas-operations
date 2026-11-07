using Microsoft.AspNetCore.Identity;

namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public class ApplicationIdentityUser : IdentityUser
    {
        internal int DomainUserId { get; set; }
    }
}
