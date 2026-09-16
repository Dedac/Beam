using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beam.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Accounts created before passwords existed may have null or duplicate usernames,
            // which would block the NOT NULL constraint and the unique index below.
            migrationBuilder.Sql(@"
UPDATE [Users]
SET [Username] = CONCAT('beam_user_', [UserId])
WHERE [Username] IS NULL OR LTRIM(RTRIM([Username])) = '';");

            migrationBuilder.Sql(@"
WITH [Duplicates] AS
(
    SELECT [UserId], [Username],
           ROW_NUMBER() OVER (PARTITION BY [Username] ORDER BY [UserId]) AS [Occurrence]
    FROM [Users]
)
UPDATE [Duplicates]
SET [Username] = CONCAT(LEFT([Username], 20), '_', [UserId])
WHERE [Occurrence] > 1;");

            migrationBuilder.Sql(@"
UPDATE [Users]
SET [Username] = LEFT([Username], 32)
WHERE LEN([Username]) > 32;");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);
        }
    }
}
