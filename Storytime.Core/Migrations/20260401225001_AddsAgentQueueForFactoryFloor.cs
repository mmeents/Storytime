using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsAgentQueueForFactoryFloor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    TargetDepth = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentQueue", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentQueue_ScheduledAt",
                table: "AgentQueue",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_AgentQueue_Status",
                table: "AgentQueue",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentQueue");
        }
    }
}
