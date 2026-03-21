using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsExtendedRelationTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemRelationTypes",
                columns: new[] { "Id", "Description", "Relation" },
                values: new object[,]
                {
                    { 5, "Scene/Beat features a character", "FeaturesCharacter" },
                    { 6, "Scene/Beat location reference", "TakesPlaceAt" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
