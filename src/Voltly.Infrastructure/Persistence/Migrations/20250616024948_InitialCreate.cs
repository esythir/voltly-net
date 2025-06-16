using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voltly.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_USERS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(180)", maxLength: 180, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    Role = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSTIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USERS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_EQUIPMENTS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    OwnerId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(250)", maxLength: 250, nullable: true),
                    DailyLimitKwh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Active = table.Column<int>(type: "NUMBER(1)", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EQUIPMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_EQUIPMENTS_TB_USERS_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "TB_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ALERTS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EquipmentId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    AlertDate = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    ConsumptionKwh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    LimitKwh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    ExceededByKwh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false, defaultValueSql: "SYSTIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ALERTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ALERTS_TB_EQUIPMENTS_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "TB_EQUIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_AUTOMATIC_ACTIONS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EquipmentId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    Type = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: false),
                    Details = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false),
                    ExecutedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AUTOMATIC_ACTIONS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_AUTOMATIC_ACTIONS_TB_EQUIPMENTS_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "TB_EQUIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONSUMPTION_LIMITS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EquipmentId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    LimitKwh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    ComputedAt = table.Column<string>(type: "NVARCHAR2(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONSUMPTION_LIMITS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CONSUMPTION_LIMITS_TB_EQUIPMENTS_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "TB_EQUIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_DAILY_REPORTS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    EquipmentId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    ReportDate = table.Column<string>(type: "NVARCHAR2(10)", nullable: false),
                    ConsumptionKwh = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    Co2EmissionKg = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    EfficiencyRating = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DAILY_REPORTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_DAILY_REPORTS_TB_EQUIPMENTS_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "TB_EQUIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_SENSORS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SerialNumber = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    EquipmentId = table.Column<long>(type: "NUMBER(19)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_SENSORS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_SENSORS_TB_EQUIPMENTS_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "TB_EQUIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ENERGY_READINGS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    SensorId = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    PowerKw = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    OccupancyPct = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TakenAt = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ENERGY_READINGS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ENERGY_READINGS_TB_SENSORS_SensorId",
                        column: x => x.SensorId,
                        principalTable: "TB_SENSORS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ALERTS_EquipmentId",
                table: "TB_ALERTS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AUTOMATIC_ACTIONS_EquipmentId",
                table: "TB_AUTOMATIC_ACTIONS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONSUMPTION_LIMITS_EquipmentId",
                table: "TB_CONSUMPTION_LIMITS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_DAILY_REPORTS_EquipmentId",
                table: "TB_DAILY_REPORTS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ENERGY_READINGS_SensorId",
                table: "TB_ENERGY_READINGS",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_EQUIPMENTS_OwnerId_Name",
                table: "TB_EQUIPMENTS",
                columns: new[] { "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_SENSORS_EquipmentId",
                table: "TB_SENSORS",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_SENSORS_SerialNumber",
                table: "TB_SENSORS",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_USERS_Email",
                table: "TB_USERS",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ALERTS");

            migrationBuilder.DropTable(
                name: "TB_AUTOMATIC_ACTIONS");

            migrationBuilder.DropTable(
                name: "TB_CONSUMPTION_LIMITS");

            migrationBuilder.DropTable(
                name: "TB_DAILY_REPORTS");

            migrationBuilder.DropTable(
                name: "TB_ENERGY_READINGS");

            migrationBuilder.DropTable(
                name: "TB_SENSORS");

            migrationBuilder.DropTable(
                name: "TB_EQUIPMENTS");

            migrationBuilder.DropTable(
                name: "TB_USERS");
        }
    }
}
