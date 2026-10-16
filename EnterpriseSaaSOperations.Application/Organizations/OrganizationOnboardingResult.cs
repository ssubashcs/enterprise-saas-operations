namespace EnterpriseSaaSOperations.Application.Organizations
{
    public sealed record OrganizationOnboardingResult(bool Succeeded,
                                                      int? OrganizationId,
                                                      int? DomainUserId,
                                                      IReadOnlyList<string> Errors);
}
