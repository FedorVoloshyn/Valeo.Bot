using Microsoft.EntityFrameworkCore.Migrations;

namespace ValeoBot.Migrations
{
    public partial class AddDoctorEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoctorID",
                table: "Orders",
                newName: "DoctorId");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Orders",
                newName: "DoctorID");
        }
    }
}
