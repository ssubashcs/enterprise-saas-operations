namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    public enum RequestStatusChangeFailure
    {
        CallerNotAuthorized,
        RequestNotFound,
        InvalidStatusValue,
        TransitionNotPermittedForRole,
        InvalidTransition
    }
}
