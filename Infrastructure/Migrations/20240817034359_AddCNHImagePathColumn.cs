using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorCyclesRentInfrastructure.Migrations
{
    public partial class AddCNHImagePathColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CNHImage",
                table: "PersonRegistrations",
                newName: "CNHImagePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CNHImagePath",
                table: "PersonRegistrations",
                newName: "CNHImage");
        }
    }
}
