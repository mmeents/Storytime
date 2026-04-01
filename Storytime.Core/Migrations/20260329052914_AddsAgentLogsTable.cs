using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsAgentLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgentLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContextItemId = table.Column<int>(type: "int", nullable: false),
                    SystemPrompt = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    UserPrompt = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    TurnCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ToolCallsSummary = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    Established = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentLogs_AgentName",
                table: "AgentLogs",
                column: "AgentName");

            migrationBuilder.CreateIndex(
                name: "IX_AgentLogs_ContextItemId",
                table: "AgentLogs",
                column: "ContextItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentLogs_Established",
                table: "AgentLogs",
                column: "Established");

            migrationBuilder.CreateIndex(
                name: "IX_AgentLogs_Success",
                table: "AgentLogs",
                column: "Success");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentLogs");
        }
    }
}
