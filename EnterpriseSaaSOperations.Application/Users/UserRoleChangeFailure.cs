namespace EnterpriseSaaSOperations.Application.Users
{
    public enum UserRoleChangeFailure
    {
        CallerNotOwner,
        UserNotFound,
        InvalidRole,
        OwnerRoleCannotBeAssigned,
        OwnerRoleCannotBeChanged
    }
}
