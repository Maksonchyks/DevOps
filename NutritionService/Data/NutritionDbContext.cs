using Microsoft.EntityFrameworkCore;
using NutritionService.Models;

namespace NutritionService.Data;

public class NutritionDbContext : DbContext
{
    public NutritionDbContext(DbContextOptions<NutritionDbContext> options) : base(options) { }

    public DbSet<Food> Foods => Set<Food>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Food>().HasData(
            new Food { Id = 1, Name = "Вівсянка", CaloriesPer100g = 68, ProteinPer100g = 2.4, CarbsPer100g = 12, FatPer100g = 1.4, Category = "breakfast" },
            new Food { Id = 2, Name = "Яйця варені", CaloriesPer100g = 155, ProteinPer100g = 13, CarbsPer100g = 1.1, FatPer100g = 11, Category = "breakfast" },
            new Food { Id = 3, Name = "Куряче філе", CaloriesPer100g = 165, ProteinPer100g = 31, CarbsPer100g = 0, FatPer100g = 3.6, Category = "lunch" },
            new Food { Id = 4, Name = "Рис варений", CaloriesPer100g = 130, ProteinPer100g = 2.7, CarbsPer100g = 28, FatPer100g = 0.3, Category = "lunch" },
            new Food { Id = 5, Name = "Лосось", CaloriesPer100g = 208, ProteinPer100g = 20, CarbsPer100g = 0, FatPer100g = 13, Category = "dinner" },
            new Food { Id = 6, Name = "Броколі", CaloriesPer100g = 34, ProteinPer100g = 2.8, CarbsPer100g = 7, FatPer100g = 0.4, Category = "dinner" },
            new Food { Id = 7, Name = "Грецькі горіхи", CaloriesPer100g = 654, ProteinPer100g = 15, CarbsPer100g = 14, FatPer100g = 65, Category = "snack" },
            new Food { Id = 8, Name = "Банан", CaloriesPer100g = 89, ProteinPer100g = 1.1, CarbsPer100g = 23, FatPer100g = 0.3, Category = "snack" },
            new Food { Id = 9, Name = "Грудка індички", CaloriesPer100g = 135, ProteinPer100g = 30, CarbsPer100g = 0, FatPer100g = 1, Category = "lunch" },
            new Food { Id = 10, Name = "Гречка варена", CaloriesPer100g = 92, ProteinPer100g = 3.4, CarbsPer100g = 20, FatPer100g = 0.6, Category = "lunch" },
            new Food { Id = 11, Name = "Сир кисломолочний 5%", CaloriesPer100g = 121, ProteinPer100g = 17, CarbsPer100g = 2, FatPer100g = 5, Category = "breakfast" },
            new Food { Id = 12, Name = "Яловичина тушкована", CaloriesPer100g = 187, ProteinPer100g = 26, CarbsPer100g = 0, FatPer100g = 9, Category = "dinner" },
            new Food { Id = 13, Name = "Солодкий перець", CaloriesPer100g = 31, ProteinPer100g = 1, CarbsPer100g = 6, FatPer100g = 0.3, Category = "dinner" },
            new Food { Id = 14, Name = "Молоко 2.5%", CaloriesPer100g = 52, ProteinPer100g = 2.9, CarbsPer100g = 4.8, FatPer100g = 2.5, Category = "breakfast" },
            new Food { Id = 15, Name = "Яблуко", CaloriesPer100g = 52, ProteinPer100g = 0.3, CarbsPer100g = 14, FatPer100g = 0.2, Category = "snack" }
        );
    }
}
