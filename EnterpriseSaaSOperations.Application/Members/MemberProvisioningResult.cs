namespace EnterpriseSaaSOperations.Application.Members
{
    /// <summary>
    /// Represents the outcome of creating a domain member and its associated authentication account.
    /// </summary>
    public sealed record MemberProvisioningResult(bool Succeeded,
                                                  int? MemberUserId,
                                                  int? TeamId,
                                                  MemberProvisioningFailure? Failure,
                                                  IReadOnlyList<string> Errors);
}
