using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTrackerApp.Data.Migrations
{
    public partial class AddTicketTypeEnumToTicketModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Ticket",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Ticket");
        }
    }
}
