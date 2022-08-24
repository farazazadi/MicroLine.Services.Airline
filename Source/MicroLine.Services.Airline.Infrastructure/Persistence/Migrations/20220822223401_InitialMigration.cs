using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Manufacturer = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "date", nullable: false),
                    PassengerSeatingCapacity_EconomyClassCapacity = table.Column<int>(type: "int", nullable: false),
                    PassengerSeatingCapacity_BusinessClassCapacity = table.Column<int>(type: "int", nullable: false),
                    PassengerSeatingCapacity_FirstClassCapacity = table.Column<int>(type: "int", nullable: false),
                    CruisingSpeed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaximumOperatingSpeed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IcaoCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    IataCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BaseUtcOffset_Hours = table.Column<int>(type: "int", nullable: false),
                    BaseUtcOffset_Minutes = table.Column<int>(type: "int", nullable: false),
                    AirportLocation_Continent = table.Column<int>(type: "int", nullable: false),
                    AirportLocation_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirportLocation_Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirportLocation_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AirportLocation_Latitude = table.Column<double>(type: "float", nullable: false),
                    AirportLocation_Longitude = table.Column<double>(type: "float", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CabinCrews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CabinCrewType = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    FullName_FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName_LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PassportNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinCrews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlightCrews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlightCrewType = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    FullName_FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName_LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PassportNumber = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address_Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightCrews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    OriginAirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationAirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledUtcDateTimeOfDeparture = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledUtcDateTimeOfArrival = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedFlightDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Prices_EconomyClass_Amount = table.Column<decimal>(type: "decimal(11,6)", precision: 11, scale: 6, nullable: false),
                    Prices_EconomyClass_Currency = table.Column<int>(type: "int", nullable: false),
                    Prices_BusinessClass_Amount = table.Column<decimal>(type: "decimal(11,6)", precision: 11, scale: 6, nullable: false),
                    Prices_BusinessClass_Currency = table.Column<int>(type: "int", nullable: false),
                    Prices_FirstClass_Amount = table.Column<decimal>(type: "decimal(11,6)", precision: 11, scale: 6, nullable: false),
                    Prices_FirstClass_Currency = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flights_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flights_Airports_DestinationAirportId",
                        column: x => x.DestinationAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flights_Airports_OriginAirportId",
                        column: x => x.OriginAirportId,
                        principalTable: "Airports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CabinCrewFlight",
                columns: table => new
                {
                    CabinCrewMembersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlightsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinCrewFlight", x => new { x.CabinCrewMembersId, x.FlightsId });
                    table.ForeignKey(
                        name: "FK_CabinCrewFlight_CabinCrews_CabinCrewMembersId",
                        column: x => x.CabinCrewMembersId,
                        principalTable: "CabinCrews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CabinCrewFlight_Flights_FlightsId",
                        column: x => x.FlightsId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightFlightCrew",
                columns: table => new
                {
                    FlightCrewMembersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlightsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightFlightCrew", x => new { x.FlightCrewMembersId, x.FlightsId });
                    table.ForeignKey(
                        name: "FK_FlightFlightCrew_FlightCrews_FlightCrewMembersId",
                        column: x => x.FlightCrewMembersId,
                        principalTable: "FlightCrews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightFlightCrew_Flights_FlightsId",
                        column: x => x.FlightsId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CabinCrewFlight_FlightsId",
                table: "CabinCrewFlight",
                column: "FlightsId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightFlightCrew_FlightsId",
                table: "FlightFlightCrew",
                column: "FlightsId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AircraftId",
                table: "Flights",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_DestinationAirportId",
                table: "Flights",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_OriginAirportId",
                table: "Flights",
                column: "OriginAirportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabinCrewFlight");

            migrationBuilder.DropTable(
                name: "FlightFlightCrew");

            migrationBuilder.DropTable(
                name: "CabinCrews");

            migrationBuilder.DropTable(
                name: "FlightCrews");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}
