using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EksiSozluk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmailColumnNameChangedToEmailAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                schema: "dbo",
                table: "User",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "dbo",
                table: "User",
                newName: "EmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "dbo",
                table: "User",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                schema: "dbo",
                table: "User",
                newName: "Email");
        }
    }
}
