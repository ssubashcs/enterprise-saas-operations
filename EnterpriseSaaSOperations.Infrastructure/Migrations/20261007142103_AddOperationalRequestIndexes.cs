using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseSaaSOperations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationalRequestIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_OrganizationId",
                table: "OperationalRequests");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_OrganizationId_AssigneeUserId",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "AssigneeUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_OrganizationId_Status",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_OrganizationId_TeamId",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "TeamId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_OrganizationId_AssigneeUserId",
                table: "OperationalRequests");

            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_OrganizationId_Status",
                table: "OperationalRequests");

            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_OrganizationId_TeamId",
                table: "OperationalRequests");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_OrganizationId",
                table: "OperationalRequests",
                column: "OrganizationId");
        }
    }
}
