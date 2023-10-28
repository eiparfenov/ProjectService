using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkplaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workplaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workplaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentModelWorkplaces",
                columns: table => new
                {
                    WorkplaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentModelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentModelWorkplaces", x => new { x.EquipmentModelId, x.WorkplaceId });
                    table.ForeignKey(
                        name: "FK_EquipmentModelWorkplaces_EquipmentModels_EquipmentModelId",
                        column: x => x.EquipmentModelId,
                        principalTable: "EquipmentModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentModelWorkplaces_Workplaces_WorkplaceId",
                        column: x => x.WorkplaceId,
                        principalTable: "Workplaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentModelWorkplaces_WorkplaceId",
                table: "EquipmentModelWorkplaces",
                column: "WorkplaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentModelWorkplaces");

            migrationBuilder.DropTable(
                name: "Workplaces");
        }
    }
}
