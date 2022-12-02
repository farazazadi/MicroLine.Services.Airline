using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeManytoManyRelationshipTypeInFlightConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabinCrewFlight_Flights_FlightsId",
                table: "CabinCrewFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightFlightCrew_Flights_FlightsId",
                table: "FlightFlightCrew");

            migrationBuilder.RenameColumn(
                name: "FlightsId",
                table: "FlightFlightCrew",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightFlightCrew_FlightsId",
                table: "FlightFlightCrew",
                newName: "IX_FlightFlightCrew_FlightId");

            migrationBuilder.RenameColumn(
                name: "FlightsId",
                table: "CabinCrewFlight",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_CabinCrewFlight_FlightsId",
                table: "CabinCrewFlight",
                newName: "IX_CabinCrewFlight_FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_CabinCrewFlight_Flights_FlightId",
                table: "CabinCrewFlight",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightFlightCrew_Flights_FlightId",
                table: "FlightFlightCrew",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CabinCrewFlight_Flights_FlightId",
                table: "CabinCrewFlight");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightFlightCrew_Flights_FlightId",
                table: "FlightFlightCrew");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "FlightFlightCrew",
                newName: "FlightsId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightFlightCrew_FlightId",
                table: "FlightFlightCrew",
                newName: "IX_FlightFlightCrew_FlightsId");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "CabinCrewFlight",
                newName: "FlightsId");

            migrationBuilder.RenameIndex(
                name: "IX_CabinCrewFlight_FlightId",
                table: "CabinCrewFlight",
                newName: "IX_CabinCrewFlight_FlightsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CabinCrewFlight_Flights_FlightsId",
                table: "CabinCrewFlight",
                column: "FlightsId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightFlightCrew_Flights_FlightsId",
                table: "FlightFlightCrew",
                column: "FlightsId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
