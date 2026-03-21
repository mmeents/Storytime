using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsInitialStorytimeSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemRelationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Relation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRelationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "{}"),
                    Established = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemRelations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    RelationTypeId = table.Column<int>(type: "int", nullable: false),
                    RelatedItemId = table.Column<int>(type: "int", nullable: false),
                    Established = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemRelations_ItemRelationTypes_RelationTypeId",
                        column: x => x.RelationTypeId,
                        principalTable: "ItemRelationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemRelations_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemRelations_Items_RelatedItemId",
                        column: x => x.RelatedItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ItemRelationTypes",
                columns: new[] { "Id", "Description", "Relation" },
                values: new object[,]
                {
                    { 1, "Parent → child (Project contains Story, Story contains Scene, etc.)", "Contains" },
                    { 2, "Scene → ordered Beat (with order stored in the Beat's Data JSON)", "HasBeat" },
                    { 3, "Beat → next Beat (optional explicit override)", "NextBeat" },
                    { 4, "Any item → global rule from the project bible", "UsesRule" }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Top-level story container (e.g. The Lost Drifter)", "Project" },
                    { 2, "A full narrative arc inside a Project", "Story" },
                    { 3, "Self-contained chunk with a clear goal", "Scene" },
                    { 4, "Single moment inside a Scene (Setup/Choice/Escalation/Climax/ResolutionHook)", "Beat" },
                    { 5, "NPC or main traveler", "Character" },
                    { 6, "Scene location", "Location" },
                    { 7, "Project bible rule", "Rule" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelations_ItemId_RelationTypeId_RelatedItemId",
                table: "ItemRelations",
                columns: new[] { "ItemId", "RelationTypeId", "RelatedItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelations_RelatedItemId",
                table: "ItemRelations",
                column: "RelatedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelations_RelationTypeId",
                table: "ItemRelations",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRelationTypes_Relation",
                table: "ItemRelationTypes",
                column: "Relation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Name",
                table: "ItemTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemRelations");

            migrationBuilder.DropTable(
                name: "ItemRelationTypes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemTypes");
        }
    }
}
