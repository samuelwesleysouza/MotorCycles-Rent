using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorCyclesRentInfrastructure.Migrations
{
    public partial class UpdateMotorcycleEntity01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Renavam",
                table: "Motorcycles",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Renavam",
                table: "Motorcycles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
