using Microsoft.EntityFrameworkCore.Migrations;

namespace IA2Assessment.Migrations
{
    public partial class TuckshopMigrationsV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLevel",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserFirstName",
                table: "Users",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserLastName",
                table: "Users",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserLastName",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserLevel",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
