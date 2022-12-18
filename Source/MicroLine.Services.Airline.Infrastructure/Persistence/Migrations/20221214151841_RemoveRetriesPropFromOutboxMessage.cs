using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroLine.Services.Airline.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRetriesPropFromOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Retries",
                table: "OutboxMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Retries",
                table: "OutboxMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
