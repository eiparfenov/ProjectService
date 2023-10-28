using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class ConnectWorkplaceToDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Workplaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Workplaces_DepartmentId",
                table: "Workplaces",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workplaces_Departments_DepartmentId",
                table: "Workplaces",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workplaces_Departments_DepartmentId",
                table: "Workplaces");

            migrationBuilder.DropIndex(
                name: "IX_Workplaces_DepartmentId",
                table: "Workplaces");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Workplaces");
        }
    }
}
