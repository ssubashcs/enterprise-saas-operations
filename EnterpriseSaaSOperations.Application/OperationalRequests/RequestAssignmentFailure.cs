namespace EnterpriseSaaSOperations.Application.OperationalRequests
{
    public enum RequestAssignmentFailure
    {
        CallerNotAuthorized,
        RequestNotFound,
        TeamNotFound,
        AssigneeNotFound,
        AssigneeNotInTeam,
        InvalidAssignment,
        InvalidRequestStatus
    }
}
