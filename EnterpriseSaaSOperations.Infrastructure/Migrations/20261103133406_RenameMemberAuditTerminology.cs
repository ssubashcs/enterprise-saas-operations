using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseSaaSOperations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameMemberAuditTerminology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE AuditEntries
                SET [Action] = N'UserRoleChanged'
                WHERE [Action] = N'MemberRoleChanged'
                """
            );

            migrationBuilder.Sql(
                """
                UPDATE dbo.AuditEntries
                SET TargetType = N'User'
                WHERE TargetType = N'Member';
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE AuditEntries
                SET [Action] = N'MemberRoleChanged'
                WHERE [Action] = N'UserRoleChanged'
                """
            );

            migrationBuilder.Sql(
                """
                UPDATE dbo.AuditEntries
                SET TargetType = N'Member'
                WHERE TargetType = N'User';
                """
            );
        }
    }
}
