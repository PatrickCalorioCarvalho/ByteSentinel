using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteSentinelServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Hostname = table.Column<string>(type: "text", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContainerMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContainerId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Cpu = table.Column<double>(type: "double precision", nullable: false),
                    Memory = table.Column<double>(type: "double precision", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuUsage = table.Column<double>(type: "double precision", nullable: false),
                    MemoryUsage = table.Column<double>(type: "double precision", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiskMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemMetricId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MountPoint = table.Column<string>(type: "text", nullable: false),
                    TotalGB = table.Column<double>(type: "double precision", nullable: false),
                    UsedGB = table.Column<double>(type: "double precision", nullable: false),
                    UsagePercent = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiskMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiskMetrics_SystemMetrics_SystemMetricId",
                        column: x => x.SystemMetricId,
                        principalTable: "SystemMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiskMetrics_SystemMetricId",
                table: "DiskMetrics",
                column: "SystemMetricId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "ContainerMetrics");

            migrationBuilder.DropTable(
                name: "DiskMetrics");

            migrationBuilder.DropTable(
                name: "SystemMetrics");
        }
    }
}
