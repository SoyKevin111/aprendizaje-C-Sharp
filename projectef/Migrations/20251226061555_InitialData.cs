using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projectef.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "task",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "category",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "category",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "CategoryId", "Descripcion", "Name", "Peso", "Tag" },
                values: new object[,]
                {
                    { new Guid("baed9063-a68c-4e94-8f55-19c2fc68f752"), "lorem ipsun door af terque cua fofvy ether who", "Actividades Personales", 70, null },
                    { new Guid("baed9063-a68c-4e94-8f55-19c2fc68f75b"), "lorem ipsun door af terque cua fofvy ether who", "Actividades Pendientes", 60, null }
                });

            migrationBuilder.InsertData(
                table: "task",
                columns: new[] { "TaskId", "CategoryId", "Create_on", "Description", "PriorityTask", "Title" },
                values: new object[,]
                {
                    { new Guid("baed9063-a68c-4e94-8f55-19c2fc68f751"), new Guid("baed9063-a68c-4e94-8f55-19c2fc68f75b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, "Pago de servicios Publicos" },
                    { new Guid("baed9063-a68c-4e94-8f55-19c2fc68f759"), new Guid("baed9063-a68c-4e94-8f55-19c2fc68f752"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, "Pago de servicios Privados" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "task",
                keyColumn: "TaskId",
                keyValue: new Guid("baed9063-a68c-4e94-8f55-19c2fc68f751"));

            migrationBuilder.DeleteData(
                table: "task",
                keyColumn: "TaskId",
                keyValue: new Guid("baed9063-a68c-4e94-8f55-19c2fc68f759"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "CategoryId",
                keyValue: new Guid("baed9063-a68c-4e94-8f55-19c2fc68f752"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "CategoryId",
                keyValue: new Guid("baed9063-a68c-4e94-8f55-19c2fc68f75b"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tag",
                table: "category",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "category",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
