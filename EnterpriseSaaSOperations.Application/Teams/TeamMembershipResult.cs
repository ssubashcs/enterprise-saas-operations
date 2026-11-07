namespace EnterpriseSaaSOperations.Application.Teams
{
    public sealed record TeamMembershipResult(bool Succeeded,
                                              int? UserId,
                                              int? TeamId,
                                              TeamMembershipFailure? Failure);
}
