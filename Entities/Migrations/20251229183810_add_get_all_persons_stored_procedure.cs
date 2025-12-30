using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class add_get_all_persons_stored_procedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string getAllPersons = """
                CREATE PROCEDURE [dbo].[PersonsGet]
                AS BEGIN
                SELECT * FROM [dbo].[Persons]
                END
                """;
            migrationBuilder.Sql(getAllPersons);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111117-aaaa-4aaa-aaaa-111111111117"),
                column: "Name",
                value: "Thomas MUller");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111120-aaaa-4aaa-aaaa-111111111120"),
                column: "Address",
                value: "SUo Paulo, Brazil");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111125-aaaa-4aaa-aaaa-111111111125"),
                column: "Address",
                value: "BrasUlia, Brazil");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string dropGetAllPersonsProcedure = """
                DROP PROCEDURE [dbo].[PersonsGet]
                """;
            migrationBuilder.Sql(dropGetAllPersonsProcedure);

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111117-aaaa-4aaa-aaaa-111111111117"),
                column: "Name",
                value: "Thomas M�ller");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111120-aaaa-4aaa-aaaa-111111111120"),
                column: "Address",
                value: "S�o Paulo, Brazil");

            migrationBuilder.UpdateData(
                table: "Persons",
                keyColumn: "Id",
                keyValue: new Guid("11111125-aaaa-4aaa-aaaa-111111111125"),
                column: "Address",
                value: "Bras�lia, Brazil");
        }
    }
}
