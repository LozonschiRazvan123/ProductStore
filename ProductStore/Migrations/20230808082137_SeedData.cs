using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductStore.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CategoryProducts",
                columns: new[] { "Id", "NameCategory" },
                values: new object[,]
                {
                    { 1, "Book" },
                    { 2, "Smartphone" },
                    { 3, "Car" },
                    { 4, "Food" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "AddressId", "Email", "Name", "Surname" },
                values: new object[,]
                {
                    { 2, 2, "tu123@gmail.com", "Popescu", "Marian" },
                    { 3, 3, "ionescu123@gmail.com", "Ionescu", "Ion" },
                    { 4, 4, "prosop@gmail.com", "Marica", "Ciprian" },
                    { 5, 5, "popa@gmail.com", "Popa", "Mihai" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerId", "DateTime" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 2, new DateTime(2022, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 2, new DateTime(2022, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 2, new DateTime(2022, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 3, new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 3, new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 4, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 5, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryProductId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Ursul pacalit de vulpe", 10 },
                    { 2, 1, "Ion", 100 },
                    { 3, 3, "Dacia", 1000 },
                    { 4, 4, "Chocolate", 11 },
                    { 5, 4, "Rice", 15 }
                });

            migrationBuilder.InsertData(
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 3, 1 },
                    { 3, 3 },
                    { 4, 3 },
                    { 5, 1 },
                    { 5, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "OrderProducts",
                keyColumns: new[] { "OrderId", "ProductId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CategoryProducts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
