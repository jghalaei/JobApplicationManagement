using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationManagement.Migrations
{
    public partial class Update_JobApplication3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JopApplicationId",
                table: "StatusHistories",
                newName: "ApplicationId");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "StatusHistories",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Roles",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistories_ApplicationId",
                table: "StatusHistories",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusHistories_JobApplications_ApplicationId",
                table: "StatusHistories",
                column: "ApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusHistories_JobApplications_ApplicationId",
                table: "StatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_StatusHistories_ApplicationId",
                table: "StatusHistories");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "StatusHistories",
                newName: "JopApplicationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StatusHistories",
                newName: "guid");
        }
    }
}
