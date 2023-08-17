using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductStore.Migrations
{
    /// <inheritdoc />
    public partial class AddUserModelAndSeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4,
                column: "UserId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5,
                column: "UserId",
                value: 5);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CustomerId", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, 1, "popescu123", 0, "Popescu Vasile" },
                    { 2, 2, "marian123", 1, "Popescu Marian" },
                    { 3, 3, "ionescu123", 1, "Ionescu Ion" },
                    { 4, 4, "prosop123", 1, "Marica Ciprian" },
                    { 5, 5, "popa123", 1, "Popa Mihai" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerId",
                table: "User",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_User_UserId",
                table: "Customers",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_User_UserId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Customers_UserId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Customers");
        }
    }
}
