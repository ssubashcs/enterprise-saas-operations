namespace EnterpriseSaaSOperations.Application.Contracts.Members
{
    public enum MemberRoleChangeFailure
    {
        CallerNotOwner,
        UserNotFound,
        InvalidRole,
        OwnerRoleCannotBeAssigned,
        OwnerRoleCannotBeChanged
    }
}
