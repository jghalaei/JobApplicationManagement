using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationManagement.Migrations
{
    public partial class Update_JobApplication5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_UserName",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_StatusHistories_JobApplications_ApplicationId",
                table: "StatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_UserName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "ApplicationId",
                table: "StatusHistories",
                newName: "JobApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_StatusHistories_ApplicationId",
                table: "StatusHistories",
                newName: "IX_StatusHistories_JobApplicationId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusHistories_JobApplications_JobApplicationId",
                table: "StatusHistories",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusHistories_JobApplications_JobApplicationId",
                table: "StatusHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "JobApplicationId",
                table: "StatusHistories",
                newName: "ApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_StatusHistories_JobApplicationId",
                table: "StatusHistories",
                newName: "IX_StatusHistories_ApplicationId");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "JobApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserName",
                table: "JobApplications",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Users_UserName",
                table: "JobApplications",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatusHistories_JobApplications_ApplicationId",
                table: "StatusHistories",
                column: "ApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
