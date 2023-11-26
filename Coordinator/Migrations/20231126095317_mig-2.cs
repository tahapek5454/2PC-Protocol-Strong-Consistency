using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("204d4ccb-da5b-4445-a88b-2686ed09b2cd"), "Stock.API" },
                    { new Guid("462c2add-12ce-4ab6-aba1-f0b0c5354541"), "Payment.API" },
                    { new Guid("be27ac8e-64ac-425f-a976-474bdef99cc4"), "Order.API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("204d4ccb-da5b-4445-a88b-2686ed09b2cd"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("462c2add-12ce-4ab6-aba1-f0b0c5354541"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("be27ac8e-64ac-425f-a976-474bdef99cc4"));
        }
    }
}
