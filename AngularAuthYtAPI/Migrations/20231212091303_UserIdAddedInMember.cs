using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularAuthYtAPI.Migrations
{
    public partial class UserIdAddedInMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_members_UserId",
                table: "members",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_members_users_UserId",
                table: "members",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_members_users_UserId",
                table: "members");

            migrationBuilder.DropIndex(
                name: "IX_members_UserId",
                table: "members");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "members");
        }
    }
}
