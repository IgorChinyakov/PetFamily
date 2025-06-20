using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Specieses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removednamingconventionslibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_breeds_species_species_id",
                schema: "species",
                table: "breeds");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "species",
                table: "species",
                newName: "species_id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "species",
                table: "breeds",
                newName: "breed_id");

            migrationBuilder.RenameIndex(
                name: "ix_breeds_species_id",
                schema: "species",
                table: "breeds",
                newName: "IX_breeds_species_id");

            migrationBuilder.AddForeignKey(
                name: "FK_breeds_species_species_id",
                schema: "species",
                table: "breeds",
                column: "species_id",
                principalSchema: "species",
                principalTable: "species",
                principalColumn: "species_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_breeds_species_species_id",
                schema: "species",
                table: "breeds");

            migrationBuilder.RenameColumn(
                name: "species_id",
                schema: "species",
                table: "species",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "breed_id",
                schema: "species",
                table: "breeds",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_breeds_species_id",
                schema: "species",
                table: "breeds",
                newName: "ix_breeds_species_id");

            migrationBuilder.AddForeignKey(
                name: "fk_breeds_species_species_id",
                schema: "species",
                table: "breeds",
                column: "species_id",
                principalSchema: "species",
                principalTable: "species",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
