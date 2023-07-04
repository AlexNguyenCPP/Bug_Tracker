using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerApp.Data.Migrations
{
    public partial class MakeTicketProjectIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Projects_ProjectId",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Ticket",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Projects_ProjectId",
                table: "Ticket",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Projects_ProjectId",
                table: "Ticket");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Projects_ProjectId",
                table: "Ticket",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
