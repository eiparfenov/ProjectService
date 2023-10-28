using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_EquipmentModel_EquipmentModelId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentModel_Departments_DepartmentId",
                table: "EquipmentModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentModel",
                table: "EquipmentModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.RenameTable(
                name: "EquipmentModel",
                newName: "EquipmentModels");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "Equipments");

            migrationBuilder.RenameIndex(
                name: "IX_EquipmentModel_DepartmentId",
                table: "EquipmentModels",
                newName: "IX_EquipmentModels_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Equipment_EquipmentModelId",
                table: "Equipments",
                newName: "IX_Equipments_EquipmentModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentModels",
                table: "EquipmentModels",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentModels_Departments_DepartmentId",
                table: "EquipmentModels",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_EquipmentModels_EquipmentModelId",
                table: "Equipments",
                column: "EquipmentModelId",
                principalTable: "EquipmentModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentModels_Departments_DepartmentId",
                table: "EquipmentModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_EquipmentModels_EquipmentModelId",
                table: "Equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentModels",
                table: "EquipmentModels");

            migrationBuilder.RenameTable(
                name: "Equipments",
                newName: "Equipment");

            migrationBuilder.RenameTable(
                name: "EquipmentModels",
                newName: "EquipmentModel");

            migrationBuilder.RenameIndex(
                name: "IX_Equipments_EquipmentModelId",
                table: "Equipment",
                newName: "IX_Equipment_EquipmentModelId");

            migrationBuilder.RenameIndex(
                name: "IX_EquipmentModels_DepartmentId",
                table: "EquipmentModel",
                newName: "IX_EquipmentModel_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentModel",
                table: "EquipmentModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_EquipmentModel_EquipmentModelId",
                table: "Equipment",
                column: "EquipmentModelId",
                principalTable: "EquipmentModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentModel_Departments_DepartmentId",
                table: "EquipmentModel",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
