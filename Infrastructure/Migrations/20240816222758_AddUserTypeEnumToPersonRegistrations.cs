using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorCyclesRentInfrastructure.Migrations
{
    public partial class AddUserTypeEnumToPersonRegistrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "PersonRegistrations");

            migrationBuilder.AddColumn<int>(
                name: "UserTypeEnum",
                table: "PersonRegistrations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserTypeEnum",
                table: "PersonRegistrations");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "PersonRegistrations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
