namespace EnterpriseSaaSOperations.Infrastructure.Identity
{
    public sealed record OrganizationOnboardingResult(bool Succeeded,
                                                      int? OrganizationId,
                                                      int? DomainUserId,
                                                      IReadOnlyList<string> Errors);
}
