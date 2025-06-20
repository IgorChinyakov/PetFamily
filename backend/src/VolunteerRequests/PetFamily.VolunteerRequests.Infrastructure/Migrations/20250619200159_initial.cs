using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.VolunteerRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "volunteer_requests");

            migrationBuilder.CreateTable(
                name: "volunteer_requests",
                schema: "volunteer_requests",
                columns: table => new
                {
                    request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    admin_id = table.Column<Guid>(type: "uuid", nullable: true, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: true, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    rejection_comment = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    request_status = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    volunteer_information = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer_requests", x => x.request_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "volunteer_requests",
                schema: "volunteer_requests");
        }
    }
}
