using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class inerst_person_stored_procedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string insertPersonStoredProcedure =
                """
                CREATE PROCEDURE [dbo].[PersonInsert]
                (@Id uniqueidentifier, @Name nvarchar(40), @Email nvarchar(40), @DateOfBirth datetime2(7), @Gender nvarchar(40), @CountryId uniqueidentifier, @Address nvarchar(40), @ReceiveNewsLetter bit)
                AS BEGIN
                INSERT INTO [dbo].[Persons] (Id, Name, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetter) VALUES (@Id, @Name, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetter);
                END
                """;
            migrationBuilder.Sql(insertPersonStoredProcedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string dropInsertPersonStoredProcedure =
                """
                DROP [dbo].[PersonInsert];
                """;
            migrationBuilder.Sql(dropInsertPersonStoredProcedure);
        }
    }
}
