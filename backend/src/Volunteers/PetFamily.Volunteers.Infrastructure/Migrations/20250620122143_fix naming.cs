using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Volunteers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixnaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpeciesId",
                schema: "pet_management",
                table: "pets",
                newName: "species_id");

            migrationBuilder.RenameColumn(
                name: "BreedId",
                schema: "pet_management",
                table: "pets",
                newName: "breed_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "species_id",
                schema: "pet_management",
                table: "pets",
                newName: "SpeciesId");

            migrationBuilder.RenameColumn(
                name: "breed_id",
                schema: "pet_management",
                table: "pets",
                newName: "BreedId");
        }
    }
}
