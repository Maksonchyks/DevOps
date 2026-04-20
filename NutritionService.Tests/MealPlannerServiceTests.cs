using FluentAssertions;
using NutritionService.Models;
using NutritionService.Services;
using Xunit;

namespace NutritionService.Tests;

public class MealPlannerServiceTests
{
    private readonly MealPlannerService _sut = new();

    private List<Food> SampleFoods() => new()
    {
        new Food { Id = 1, Name = "Вівсянка",        CaloriesPer100g = 68,  ProteinPer100g = 2.4, CarbsPer100g = 12, FatPer100g = 1.4, Category = "breakfast" },
        new Food { Id = 2, Name = "Яйця",             CaloriesPer100g = 155, ProteinPer100g = 13,  CarbsPer100g = 1.1, FatPer100g = 11, Category = "breakfast" },
        new Food { Id = 3, Name = "Куряче філе",      CaloriesPer100g = 165, ProteinPer100g = 31,  CarbsPer100g = 0,  FatPer100g = 3.6, Category = "lunch" },
        new Food { Id = 4, Name = "Рис",              CaloriesPer100g = 130, ProteinPer100g = 2.7, CarbsPer100g = 28, FatPer100g = 0.3, Category = "lunch" },
        new Food { Id = 5, Name = "Лосось",           CaloriesPer100g = 208, ProteinPer100g = 20,  CarbsPer100g = 0,  FatPer100g = 13,  Category = "dinner" },
        new Food { Id = 6, Name = "Броколі",          CaloriesPer100g = 34,  ProteinPer100g = 2.8, CarbsPer100g = 7,  FatPer100g = 0.4, Category = "dinner" },
        new Food { Id = 7, Name = "Горіхи",           CaloriesPer100g = 654, ProteinPer100g = 15,  CarbsPer100g = 14, FatPer100g = 65,  Category = "snack" },
        new Food { Id = 8, Name = "Банан",            CaloriesPer100g = 89,  ProteinPer100g = 1.1, CarbsPer100g = 23, FatPer100g = 0.3, Category = "snack" },
    };

    // --- AdjustCaloriesForGoal ---

    [Fact]
    public void AdjustCalories_Maintain_ReturnsSame()
    {
        _sut.AdjustCaloriesForGoal(2000, "maintain").Should().Be(2000);
    }

    [Fact]
    public void AdjustCalories_Lose_Returns80Percent()
    {
        _sut.AdjustCaloriesForGoal(2000, "lose").Should().Be(1600);
    }

    [Fact]
    public void AdjustCalories_Gain_Returns120Percent()
    {
        _sut.AdjustCaloriesForGoal(2000, "gain").Should().Be(2400);
    }

    [Theory]
    [InlineData("LOSE")]
    [InlineData("Lose")]
    public void AdjustCalories_GoalIsCaseInsensitive(string goal)
    {
        _sut.AdjustCaloriesForGoal(2000, goal).Should().Be(1600);
    }

    // --- BuildMealEntry ---

    [Fact]
    public void BuildMealEntry_CalculatesCorrectMacros()
    {
        var food = new Food
        {
            Name = "Тест", CaloriesPer100g = 200, ProteinPer100g = 20,
            CarbsPer100g = 10, FatPer100g = 5
        };

        var entry = _sut.BuildMealEntry(food, 200);

        entry.Calories.Should().Be(400);
        entry.Protein.Should().Be(40);
        entry.Carbs.Should().Be(20);
        entry.Fat.Should().Be(10);
        entry.GramsAmount.Should().Be(200);
    }

    [Fact]
    public void BuildMealEntry_50Grams_HalfMacros()
    {
        var food = new Food
        {
            Name = "Тест", CaloriesPer100g = 100, ProteinPer100g = 10,
            CarbsPer100g = 10, FatPer100g = 10
        };

        var entry = _sut.BuildMealEntry(food, 50);

        entry.Calories.Should().Be(50);
        entry.Protein.Should().Be(5);
    }

    // --- GeneratePlan ---

    [Fact]
    public void GeneratePlan_OneDayMaintain_HasCorrectDayCount()
    {
        var plan = _sut.GeneratePlan(SampleFoods(), new MealPlanRequest
        {
            TargetCalories = 2000, Days = 1, Goal = "maintain"
        });

        plan.Days.Should().HaveCount(1);
    }

    [Fact]
    public void GeneratePlan_ThreeDays_HasThreeDays()
    {
        var plan = _sut.GeneratePlan(SampleFoods(), new MealPlanRequest
        {
            TargetCalories = 2000, Days = 3, Goal = "maintain"
        });

        plan.Days.Should().HaveCount(3);
        plan.Days.Select(d => d.DayNumber).Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void GeneratePlan_EachDay_HasAllMealGroups()
    {
        var plan = _sut.GeneratePlan(SampleFoods(), new MealPlanRequest
        {
            TargetCalories = 2000, Days = 1, Goal = "maintain"
        });

        var day = plan.Days[0];
        day.Breakfast.Should().NotBeEmpty();
        day.Lunch.Should().NotBeEmpty();
        day.Dinner.Should().NotBeEmpty();
        day.Snacks.Should().NotBeEmpty();
    }

    [Fact]
    public void GeneratePlan_TotalCalories_IsPositive()
    {
        var plan = _sut.GeneratePlan(SampleFoods(), new MealPlanRequest
        {
            TargetCalories = 2000, Days = 1, Goal = "maintain"
        });

        plan.Days[0].TotalCalories.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GeneratePlan_GoalLose_TargetCaloriesReducedInPlan()
    {
        var plan = _sut.GeneratePlan(SampleFoods(), new MealPlanRequest
        {
            TargetCalories = 2000, Days = 1, Goal = "lose"
        });

        plan.TargetCalories.Should().Be(1600);
    }

    [Fact]
    public void GeneratePlan_EmptyFoodList_ReturnsEmptyMeals()
    {
        var plan = _sut.GeneratePlan(new List<Food>(), new MealPlanRequest
        {
            TargetCalories = 2000, Days = 1, Goal = "maintain"
        });

        plan.Days[0].Breakfast.Should().BeEmpty();
        plan.Days[0].Lunch.Should().BeEmpty();
    }
}
