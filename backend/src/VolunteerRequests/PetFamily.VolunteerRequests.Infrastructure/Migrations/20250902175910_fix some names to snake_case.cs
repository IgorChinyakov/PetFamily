using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.VolunteerRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixsomenamestosnake_case : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "volunteer_requests",
                table: "OutboxMessages");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "volunteer_requests",
                newName: "outbox_messages",
                newSchema: "volunteer_requests");

            migrationBuilder.RenameColumn(
                name: "Error",
                schema: "volunteer_requests",
                table: "outbox_messages",
                newName: "error");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                schema: "volunteer_requests",
                table: "outbox_messages",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                schema: "volunteer_requests",
                table: "outbox_messages");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "volunteer_requests",
                newName: "OutboxMessages",
                newSchema: "volunteer_requests");

            migrationBuilder.RenameColumn(
                name: "error",
                schema: "volunteer_requests",
                table: "OutboxMessages",
                newName: "Error");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "volunteer_requests",
                table: "OutboxMessages",
                column: "id");
        }
    }
}
