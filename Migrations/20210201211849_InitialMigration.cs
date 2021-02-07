using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherApi.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TemperatureC = table.Column<int>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 1, new DateTime(2021, 2, 2, 13, 18, 48, 714, DateTimeKind.Local).AddTicks(8988), "Mild", 45 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 2, new DateTime(2021, 2, 3, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(3955), "Hot", 34 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 3, new DateTime(2021, 2, 4, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(3980), "Balmy", 13 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 4, new DateTime(2021, 2, 5, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(3985), "Scorching", 12 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 5, new DateTime(2021, 2, 6, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(3987), "Freezing", -6 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 6, new DateTime(2021, 2, 7, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(3991), "Scorching", 34 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 7, new DateTime(2021, 2, 8, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(3997), "Bracing", -18 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 8, new DateTime(2021, 2, 9, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(4000), "Warm", 26 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 9, new DateTime(2021, 2, 10, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(4003), "Balmy", 43 });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { 10, new DateTime(2021, 2, 11, 13, 18, 48, 718, DateTimeKind.Local).AddTicks(4006), "Hot", 15 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");
        }
    }
}
