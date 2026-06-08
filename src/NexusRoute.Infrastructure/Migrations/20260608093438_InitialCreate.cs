using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusRoute.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CurrentLatitude = table.Column<double>(type: "REAL", nullable: true),
                    CurrentLongitude = table.Column<double>(type: "REAL", nullable: true),
                    LastTelemetryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionQuotas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PeriodName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TargetTonnage = table.Column<double>(type: "REAL", nullable: false),
                    TargetGrade = table.Column<double>(type: "REAL", nullable: true),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    ActualTonnage = table.Column<double>(type: "REAL", nullable: false),
                    ActualGrade = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionQuotas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    StartLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    EndLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    StartLatitude = table.Column<double>(type: "REAL", nullable: false),
                    StartLongitude = table.Column<double>(type: "REAL", nullable: false),
                    EndLatitude = table.Column<double>(type: "REAL", nullable: false),
                    EndLongitude = table.Column<double>(type: "REAL", nullable: false),
                    EstimatedDistanceKm = table.Column<double>(type: "REAL", nullable: false),
                    EstimatedDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    GeofenceRadiusKm = table.Column<double>(type: "REAL", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OreMovementLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EventType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MaterialType = table.Column<int>(type: "INTEGER", nullable: false),
                    TonnageEstimate = table.Column<double>(type: "REAL", nullable: false),
                    GradeEstimate = table.Column<double>(type: "REAL", nullable: true),
                    SourceLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DestinationLocation = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    SourceLatitude = table.Column<double>(type: "REAL", nullable: false),
                    SourceLongitude = table.Column<double>(type: "REAL", nullable: false),
                    DestinationLatitude = table.Column<double>(type: "REAL", nullable: true),
                    DestinationLongitude = table.Column<double>(type: "REAL", nullable: true),
                    EventTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CycleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    OperatorName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OreMovementLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OreMovementLogs_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telemetry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EngineTemperatureCelsius = table.Column<double>(type: "REAL", nullable: false),
                    PayloadWeightTonnes = table.Column<double>(type: "REAL", nullable: false),
                    TirePressurePsi = table.Column<double>(type: "REAL", nullable: false),
                    FuelLevelPercentage = table.Column<double>(type: "REAL", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Altitude = table.Column<double>(type: "REAL", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telemetry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telemetry_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Checkpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RouteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SequenceNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    GeofenceRadiusKm = table.Column<double>(type: "REAL", nullable: false),
                    ExpectedTimeFromStart = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    AllowedDeviationTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IsMandatory = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checkpoints_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Convoys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConvoyCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RouteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LeadAssetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CargoType = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CargoValueUsd = table.Column<double>(type: "REAL", nullable: false),
                    ScheduledDepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualDepartureTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsHighPriority = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convoys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Convoys_Assets_LeadAssetId",
                        column: x => x.LeadAssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Convoys_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Severity = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ConvoyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcknowledgedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AcknowledgedBy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Assets_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Alerts_Convoys_ConvoyId",
                        column: x => x.ConvoyId,
                        principalTable: "Convoys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ConvoyCheckpointLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConvoyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CheckpointId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsMissed = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsOverdue = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvoyCheckpointLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvoyCheckpointLogs_Checkpoints_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "Checkpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConvoyCheckpointLogs_Convoys_ConvoyId",
                        column: x => x.ConvoyId,
                        principalTable: "Convoys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_AssetId",
                table: "Alerts",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_ConvoyId",
                table: "Alerts",
                column: "ConvoyId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CreatedAt",
                table: "Alerts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_IsActive",
                table: "Alerts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_IsActive_Severity",
                table: "Alerts",
                columns: new[] { "IsActive", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssetCode",
                table: "Assets",
                column: "AssetCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_LastTelemetryTime",
                table: "Assets",
                column: "LastTelemetryTime");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Status",
                table: "Assets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_Type",
                table: "Assets",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Checkpoints_RouteId",
                table: "Checkpoints",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkpoints_RouteId_SequenceNumber",
                table: "Checkpoints",
                columns: new[] { "RouteId", "SequenceNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_ConvoyCheckpointLogs_CheckpointId",
                table: "ConvoyCheckpointLogs",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvoyCheckpointLogs_ConvoyId",
                table: "ConvoyCheckpointLogs",
                column: "ConvoyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvoyCheckpointLogs_ConvoyId_CheckpointId",
                table: "ConvoyCheckpointLogs",
                columns: new[] { "ConvoyId", "CheckpointId" });

            migrationBuilder.CreateIndex(
                name: "IX_Convoys_ConvoyCode",
                table: "Convoys",
                column: "ConvoyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Convoys_LeadAssetId",
                table: "Convoys",
                column: "LeadAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Convoys_RouteId",
                table: "Convoys",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Convoys_ScheduledDepartureTime",
                table: "Convoys",
                column: "ScheduledDepartureTime");

            migrationBuilder.CreateIndex(
                name: "IX_Convoys_Status",
                table: "Convoys",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OreMovementLogs_AssetId",
                table: "OreMovementLogs",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_OreMovementLogs_AssetId_EventTime",
                table: "OreMovementLogs",
                columns: new[] { "AssetId", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_OreMovementLogs_CycleId",
                table: "OreMovementLogs",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_OreMovementLogs_EventTime",
                table: "OreMovementLogs",
                column: "EventTime");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionQuotas_EndDate",
                table: "ProductionQuotas",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionQuotas_StartDate",
                table: "ProductionQuotas",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionQuotas_StartDate_EndDate",
                table: "ProductionQuotas",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_IsActive",
                table: "Routes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Name",
                table: "Routes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Telemetry_AssetId",
                table: "Telemetry",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Telemetry_AssetId_Timestamp",
                table: "Telemetry",
                columns: new[] { "AssetId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Telemetry_Timestamp",
                table: "Telemetry",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "ConvoyCheckpointLogs");

            migrationBuilder.DropTable(
                name: "OreMovementLogs");

            migrationBuilder.DropTable(
                name: "ProductionQuotas");

            migrationBuilder.DropTable(
                name: "Telemetry");

            migrationBuilder.DropTable(
                name: "Checkpoints");

            migrationBuilder.DropTable(
                name: "Convoys");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
