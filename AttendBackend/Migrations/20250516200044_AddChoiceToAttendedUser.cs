using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddChoiceToAttendedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Choice",
                table: "AttendedUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponseDate",
                table: "AttendedUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Choice",
                table: "AttendedUsers");

            migrationBuilder.DropColumn(
                name: "ResponseDate",
                table: "AttendedUsers");
        }
    }
}
