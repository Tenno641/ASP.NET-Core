using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    ReceiveNewsLetter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("a1e1f1d0-1111-4a2b-8888-123456789001"), "Egypt" },
                    { new Guid("b2e2f2d0-2222-4b3c-9999-123456789002"), "Germany" },
                    { new Guid("c3e3f3d0-3333-4c4d-aaaa-123456789003"), "France" },
                    { new Guid("d4e4f4d0-4444-4d5e-bbbb-123456789004"), "Japan" },
                    { new Guid("e5e5f5d0-5555-4e6f-cccc-123456789005"), "Brazil" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "Name", "ReceiveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-4aaa-aaaa-111111111111"), "Cairo, Egypt", new Guid("a1e1f1d0-1111-4a2b-8888-123456789001"), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed.ali@example.com", "Male", "Ahmed Ali", true },
                    { new Guid("11111112-aaaa-4aaa-aaaa-111111111112"), "Berlin, Germany", new Guid("b2e2f2d0-2222-4b3c-9999-123456789002"), new DateTime(1985, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara.hassan@example.com", "Female", "Sara Hassan", false },
                    { new Guid("11111113-aaaa-4aaa-aaaa-111111111113"), "Paris, France", new Guid("c3e3f3d0-3333-4c4d-aaaa-123456789003"), new DateTime(1992, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.smith@example.com", "Male", "John Smith", true },
                    { new Guid("11111114-aaaa-4aaa-aaaa-111111111114"), "Tokyo, Japan", new Guid("d4e4f4d0-4444-4d5e-bbbb-123456789004"), new DateTime(1995, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "yuki.tanaka@example.com", "Female", "Yuki Tanaka", true },
                    { new Guid("11111115-aaaa-4aaa-aaaa-111111111115"), "Rio de Janeiro, Brazil", new Guid("e5e5f5d0-5555-4e6f-cccc-123456789005"), new DateTime(1988, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "carlos.silva@example.com", "Male", "Carlos Silva", false },
                    { new Guid("11111116-aaaa-4aaa-aaaa-111111111116"), "Alexandria, Egypt", new Guid("a1e1f1d0-1111-4a2b-8888-123456789001"), new DateTime(1993, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "laila.mohamed@example.com", "Female", "Laila Mohamed", true },
                    { new Guid("11111117-aaaa-4aaa-aaaa-111111111117"), "Munich, Germany", new Guid("b2e2f2d0-2222-4b3c-9999-123456789002"), new DateTime(1991, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "thomas.muller@example.com", "Male", "Thomas M�ller", false },
                    { new Guid("11111118-aaaa-4aaa-aaaa-111111111118"), "Lyon, France", new Guid("c3e3f3d0-3333-4c4d-aaaa-123456789003"), new DateTime(1990, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "marie.dubois@example.com", "Female", "Marie Dubois", true },
                    { new Guid("11111119-aaaa-4aaa-aaaa-111111111119"), "Osaka, Japan", new Guid("d4e4f4d0-4444-4d5e-bbbb-123456789004"), new DateTime(1994, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "kenji.sato@example.com", "Male", "Kenji Sato", true },
                    { new Guid("11111120-aaaa-4aaa-aaaa-111111111120"), "S�o Paulo, Brazil", new Guid("e5e5f5d0-5555-4e6f-cccc-123456789005"), new DateTime(1989, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "ana.pereira@example.com", "Female", "Ana Pereira", false },
                    { new Guid("11111121-aaaa-4aaa-aaaa-111111111121"), "Giza, Egypt", new Guid("a1e1f1d0-1111-4a2b-8888-123456789001"), new DateTime(1992, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohamed.hassan@example.com", "Male", "Mohamed Hassan", true },
                    { new Guid("11111122-aaaa-4aaa-aaaa-111111111122"), "Hamburg, Germany", new Guid("b2e2f2d0-2222-4b3c-9999-123456789002"), new DateTime(1987, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "julia.schmidt@example.com", "Female", "Julia Schmidt", false },
                    { new Guid("11111123-aaaa-4aaa-aaaa-111111111123"), "Marseille, France", new Guid("c3e3f3d0-3333-4c4d-aaaa-123456789003"), new DateTime(1991, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "pierre.martin@example.com", "Male", "Pierre Martin", true },
                    { new Guid("11111124-aaaa-4aaa-aaaa-111111111124"), "Kyoto, Japan", new Guid("d4e4f4d0-4444-4d5e-bbbb-123456789004"), new DateTime(1993, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "hiroko.nakamura@example.com", "Female", "Hiroko Nakamura", true },
                    { new Guid("11111125-aaaa-4aaa-aaaa-111111111125"), "Bras�lia, Brazil", new Guid("e5e5f5d0-5555-4e6f-cccc-123456789005"), new DateTime(1990, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "felipe.costa@example.com", "Male", "Felipe Costa", false },
                    { new Guid("11111126-aaaa-4aaa-aaaa-111111111126"), "Aswan, Egypt", new Guid("a1e1f1d0-1111-4a2b-8888-123456789001"), new DateTime(1995, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "nour.ahmed@example.com", "Female", "Nour Ahmed", true },
                    { new Guid("11111127-aaaa-4aaa-aaaa-111111111127"), "Frankfurt, Germany", new Guid("b2e2f2d0-2222-4b3c-9999-123456789002"), new DateTime(1988, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "markus.lehmann@example.com", "Male", "Markus Lehmann", false },
                    { new Guid("11111128-aaaa-4aaa-aaaa-111111111128"), "Nice, France", new Guid("c3e3f3d0-3333-4c4d-aaaa-123456789003"), new DateTime(1992, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "claire.moreau@example.com", "Female", "Claire Moreau", true },
                    { new Guid("11111129-aaaa-4aaa-aaaa-111111111129"), "Hiroshima, Japan", new Guid("d4e4f4d0-4444-4d5e-bbbb-123456789004"), new DateTime(1994, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "takashi.yamamoto@example.com", "Male", "Takashi Yamamoto", true },
                    { new Guid("11111130-aaaa-4aaa-aaaa-111111111130"), "Salvador, Brazil", new Guid("e5e5f5d0-5555-4e6f-cccc-123456789005"), new DateTime(1989, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "juliana.lima@example.com", "Female", "Juliana Lima", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
