using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClayService.Infrastructure.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    TagCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    DoorId = table.Column<long>(type: "bigint", nullable: false),
                    SourceType = table.Column<byte>(type: "tinyint", nullable: false),
                    OperationResult = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "offices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doors_offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalTagId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_PhysicalTags_PhysicalTagId",
                        column: x => x.PhysicalTagId,
                        principalTable: "PhysicalTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DoorUser",
                columns: table => new
                {
                    DoorsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorUser", x => new { x.DoorsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_DoorUser_Doors_DoorsId",
                        column: x => x.DoorsId,
                        principalTable: "Doors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficeUser",
                columns: table => new
                {
                    OfficesId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeUser", x => new { x.OfficesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_OfficeUser_offices_OfficesId",
                        column: x => x.OfficesId,
                        principalTable: "offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doors_OfficeId",
                table: "Doors",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorUser_UsersId",
                table: "DoorUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_EventHistories_DoorId_CreatedDate",
                table: "EventHistories",
                columns: new[] { "DoorId", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EventHistories_UserId_CreatedDate",
                table: "EventHistories",
                columns: new[] { "UserId", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeUser_UsersId",
                table: "OfficeUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalTags_TagCode",
                table: "PhysicalTags",
                column: "TagCode",
                unique: true,
                filter: "[TagCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalTags_TagCode_CreatedDate",
                table: "PhysicalTags",
                columns: new[] { "TagCode", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhysicalTagId",
                table: "Users",
                column: "PhysicalTagId",
                unique: true,
                filter: "[PhysicalTagId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoorUser");

            migrationBuilder.DropTable(
                name: "EventHistories");

            migrationBuilder.DropTable(
                name: "OfficeUser");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "offices");

            migrationBuilder.DropTable(
                name: "PhysicalTags");
        }
    }
}
