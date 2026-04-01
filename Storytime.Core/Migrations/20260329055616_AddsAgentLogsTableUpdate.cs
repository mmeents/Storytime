using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storytime.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddsAgentLogsTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnCount",
                table: "AgentLogs");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "AgentLogs",
                type: "nvarchar(max)",
                maxLength: -1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: -1);

            migrationBuilder.AddColumn<string>(
                name: "RawResponse",
                table: "AgentLogs",
                type: "nvarchar(max)",
                maxLength: -1,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawResponse",
                table: "AgentLogs");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorMessage",
                table: "AgentLogs",
                type: "nvarchar(max)",
                maxLength: -1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: -1,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TurnCount",
                table: "AgentLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
