namespace EnterpriseSaaSOperations.Application.Members
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
