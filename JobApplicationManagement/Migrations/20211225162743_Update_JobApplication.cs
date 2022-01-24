using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationManagement.Migrations
{
    public partial class Update_JobApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "guid",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "JobFieldId",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "JobApplications",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "JobField",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "JobApplications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "Id");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Users_UserName",
                table: "JobApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_UserName",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "JobField",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JobApplications",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "JobFieldId",
                table: "JobApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobApplications",
                table: "JobApplications",
                column: "guid");

            migrationBuilder.CreateTable(
                name: "JobFields",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobFields", x => x.guid);
                });
        }
    }
}
