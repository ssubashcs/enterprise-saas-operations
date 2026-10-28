using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseSaaSOperations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationScopedAuditTargetIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditEntries_TargetType_TargetId",
                table: "AuditEntries");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_OrganizationId_TargetType_TargetId",
                table: "AuditEntries",
                columns: new[] { "OrganizationId", "TargetType", "TargetId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditEntries_OrganizationId_TargetType_TargetId",
                table: "AuditEntries");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_TargetType_TargetId",
                table: "AuditEntries",
                columns: new[] { "TargetType", "TargetId" });
        }
    }
}
