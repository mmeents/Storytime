using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsNewRelationTypesNarration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemRelationTypes",
                columns: new[] { "Id", "Description", "Relation" },
                values: new object[] { 11, "CallSheet → Narration (Defines the narration for a scene, with rank defining order if multiple)", "Narrates" });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 12, "Narration item for call sheets", "Narration" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
