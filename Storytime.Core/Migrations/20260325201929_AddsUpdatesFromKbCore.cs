using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsUpdatesFromKbCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemRelations_ItemId_RelationTypeId_RelatedItemId",
                table: "ItemRelations");

            migrationBuilder.AlterColumn<int>(
                name: "RelatedItemId",
                table: "ItemRelations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "ItemRelationTypes",
                columns: new[] { "Id", "Description", "Relation" },
                values: new object[] { 7, "Any item → tone reference", "UsesTone" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelations_ItemId_RelationTypeId_RelatedItemId",
                table: "ItemRelations",
                columns: new[] { "ItemId", "RelationTypeId", "RelatedItemId" },
                unique: true,
                filter: "[RelatedItemId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemRelations_ItemId_RelationTypeId_RelatedItemId",
                table: "ItemRelations");

            migrationBuilder.DeleteData(
                table: "ItemRelationTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AlterColumn<int>(
                name: "RelatedItemId",
                table: "ItemRelations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelations_ItemId_RelationTypeId_RelatedItemId",
                table: "ItemRelations",
                columns: new[] { "ItemId", "RelationTypeId", "RelatedItemId" },
                unique: true);
        }
    }
}
