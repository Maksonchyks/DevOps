using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NutritionService.Migrations
{
    /// <inheritdoc />
    public partial class InitialRealCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CaloriesPer100g = table.Column<double>(type: "double precision", nullable: false),
                    ProteinPer100g = table.Column<double>(type: "double precision", nullable: false),
                    CarbsPer100g = table.Column<double>(type: "double precision", nullable: false),
                    FatPer100g = table.Column<double>(type: "double precision", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "CaloriesPer100g", "CarbsPer100g", "Category", "FatPer100g", "Name", "ProteinPer100g" },
                values: new object[,]
                {
                    { 1, 68.0, 12.0, "breakfast", 1.3999999999999999, "Вівсянка", 2.3999999999999999 },
                    { 2, 155.0, 1.1000000000000001, "breakfast", 11.0, "Яйця варені", 13.0 },
                    { 3, 165.0, 0.0, "lunch", 3.6000000000000001, "Куряче філе", 31.0 },
                    { 4, 130.0, 28.0, "lunch", 0.29999999999999999, "Рис варений", 2.7000000000000002 },
                    { 5, 208.0, 0.0, "dinner", 13.0, "Лосось", 20.0 },
                    { 6, 34.0, 7.0, "dinner", 0.40000000000000002, "Броколі", 2.7999999999999998 },
                    { 7, 654.0, 14.0, "snack", 65.0, "Грецькі горіхи", 15.0 },
                    { 8, 89.0, 23.0, "snack", 0.29999999999999999, "Банан", 1.1000000000000001 },
                    { 9, 135.0, 0.0, "lunch", 1.0, "Грудка індички", 30.0 },
                    { 10, 92.0, 20.0, "lunch", 0.59999999999999998, "Гречка варена", 3.3999999999999999 },
                    { 11, 121.0, 2.0, "breakfast", 5.0, "Сир кисломолочний 5%", 17.0 },
                    { 12, 187.0, 0.0, "dinner", 9.0, "Яловичина тушкована", 26.0 },
                    { 13, 31.0, 6.0, "dinner", 0.29999999999999999, "Солодкий перець", 1.0 },
                    { 14, 52.0, 4.7999999999999998, "breakfast", 2.5, "Молоко 2.5%", 2.8999999999999999 },
                    { 15, 52.0, 14.0, "snack", 0.20000000000000001, "Яблуко", 0.29999999999999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}
