using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsKbCore105ChangesandNewTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "ItemRelations",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ItemRelationTypes",
                columns: new[] { "Id", "Description", "Relation" },
                values: new object[,]
                {
                    { 8, "Scene → CallSheet (Director's interpretation of curated beats)", "DirectedAs" },
                    { 9, "CallSheet → Performance, or Performance → Deliverable", "Produces" },
                    { 10, "CallSheet → Character (Rank on the relation defines cast order)", "HasRole" }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 8, "Project bible tone/mood reference", "Tone" },
                    { 9, "Director's ordered cast sequence for a Scene", "CallSheet" },
                    { 10, "Executed result of a CallSheet by character agents", "Performance" },
                    { 11, "Final rendered output — prose, podcast, video, etc.", "Deliverable" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ItemTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "ItemRelations");

            migrationBuilder.InsertData(
                table: "ItemRelationTypes",
                columns: new[] { "Id", "Description", "Relation" },
                values: new object[,]
                {
                    { 2, "Scene → ordered Beat (with order stored in the Beat's Data JSON)", "HasBeat" },
                    { 3, "Beat → next Beat (optional explicit override)", "NextBeat" }
                });
        }
    }
}
