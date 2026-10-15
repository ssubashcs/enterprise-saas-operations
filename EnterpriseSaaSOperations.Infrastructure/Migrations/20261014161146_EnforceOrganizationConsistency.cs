using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseSaaSOperations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnforceOrganizationConsistency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationalRequests_Teams_TeamId",
                table: "OperationalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationalRequests_Users_AssigneeUserId",
                table: "OperationalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationalRequests_Users_RequesterUserId",
                table: "OperationalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Teams_TeamId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TeamId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_AssigneeUserId",
                table: "OperationalRequests");

            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_RequesterUserId",
                table: "OperationalRequests");

            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_TeamId",
                table: "OperationalRequests");
                
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_OrganizationId_Id",
                table: "Users",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Teams_OrganizationId_Id",
                table: "Teams",
                columns: new[] { "OrganizationId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId_TeamId",
                table: "Users",
                columns: new[] { "OrganizationId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_OrganizationId_RequesterUserId",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "RequesterUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalRequests_Teams_OrganizationId_TeamId",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "TeamId" },
                principalTable: "Teams",
                principalColumns: new[] { "OrganizationId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalRequests_Users_OrganizationId_AssigneeUserId",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "AssigneeUserId" },
                principalTable: "Users",
                principalColumns: new[] { "OrganizationId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalRequests_Users_OrganizationId_RequesterUserId",
                table: "OperationalRequests",
                columns: new[] { "OrganizationId", "RequesterUserId" },
                principalTable: "Users",
                principalColumns: new[] { "OrganizationId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Teams_OrganizationId_TeamId",
                table: "Users",
                columns: new[] { "OrganizationId", "TeamId" },
                principalTable: "Teams",
                principalColumns: new[] { "OrganizationId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationalRequests_Teams_OrganizationId_TeamId",
                table: "OperationalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationalRequests_Users_OrganizationId_AssigneeUserId",
                table: "OperationalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationalRequests_Users_OrganizationId_RequesterUserId",
                table: "OperationalRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Teams_OrganizationId_TeamId",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_OrganizationId_Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationId_TeamId",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Teams_OrganizationId_Id",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_OperationalRequests_OrganizationId_RequesterUserId",
                table: "OperationalRequests");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OrganizationId",
                table: "Teams",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_AssigneeUserId",
                table: "OperationalRequests",
                column: "AssigneeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_RequesterUserId",
                table: "OperationalRequests",
                column: "RequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationalRequests_TeamId",
                table: "OperationalRequests",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalRequests_Teams_TeamId",
                table: "OperationalRequests",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalRequests_Users_AssigneeUserId",
                table: "OperationalRequests",
                column: "AssigneeUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationalRequests_Users_RequesterUserId",
                table: "OperationalRequests",
                column: "RequesterUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Teams_TeamId",
                table: "Users",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
