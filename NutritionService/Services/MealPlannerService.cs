using NutritionService.Models;

namespace NutritionService.Services;

public interface IMealPlannerService
{
    MealPlan GeneratePlan(List<Food> foods, MealPlanRequest request);
    MealEntry BuildMealEntry(Food food, double grams);
    double AdjustCaloriesForGoal(int targetCalories, string goal);
}

public class MealPlannerService : IMealPlannerService
{
    public double AdjustCaloriesForGoal(int targetCalories, string goal) =>
        goal.ToLower() switch
        {
            "lose"    => targetCalories * 0.8,
            "gain"    => targetCalories * 1.2,
            _         => targetCalories
        };

    public MealEntry BuildMealEntry(Food food, double grams)
    {
        var ratio = grams / 100.0;
        return new MealEntry
        {
            FoodName = food.Name,
            GramsAmount = grams,
            Calories = Math.Round(food.CaloriesPer100g * ratio, 1),
            Protein  = Math.Round(food.ProteinPer100g  * ratio, 1),
            Carbs    = Math.Round(food.CarbsPer100g    * ratio, 1),
            Fat      = Math.Round(food.FatPer100g      * ratio, 1),
        };
    }

    public MealPlan GeneratePlan(List<Food> foods, MealPlanRequest request)
    {
        var adjustedCalories = AdjustCaloriesForGoal(request.TargetCalories, request.Goal);

        // split calories: 25% breakfast, 35% lunch, 30% dinner, 10% snack
        double breakfastCal = adjustedCalories * 0.25;
        double lunchCal     = adjustedCalories * 0.35;
        double dinnerCal    = adjustedCalories * 0.30;
        double snackCal     = adjustedCalories * 0.10;

        var byCategory = foods.GroupBy(f => f.Category)
            .ToDictionary(g => g.Key, g => g.ToList());

        var rng = new Random();
        var plan = new MealPlan { TargetCalories = (int)adjustedCalories, Goal = request.Goal };

        for (int day = 1; day <= request.Days; day++)
        {
            var dayPlan = new DayPlan { DayNumber = day };

            dayPlan.Breakfast = BuildMealsForCalories(byCategory.GetValueOrDefault("breakfast", new()), breakfastCal, rng);
            dayPlan.Lunch     = BuildMealsForCalories(byCategory.GetValueOrDefault("lunch",     new()), lunchCal,     rng);
            dayPlan.Dinner    = BuildMealsForCalories(byCategory.GetValueOrDefault("dinner",    new()), dinnerCal,    rng);
            dayPlan.Snacks    = BuildMealsForCalories(byCategory.GetValueOrDefault("snack",     new()), snackCal,     rng);

            var allEntries = dayPlan.Breakfast.Concat(dayPlan.Lunch).Concat(dayPlan.Dinner).Concat(dayPlan.Snacks);
            dayPlan.TotalCalories = Math.Round(allEntries.Sum(e => e.Calories), 1);
            dayPlan.TotalProtein  = Math.Round(allEntries.Sum(e => e.Protein),  1);
            dayPlan.TotalCarbs    = Math.Round(allEntries.Sum(e => e.Carbs),    1);
            dayPlan.TotalFat      = Math.Round(allEntries.Sum(e => e.Fat),      1);

            plan.Days.Add(dayPlan);
        }

        return plan;
    }

    private List<MealEntry> BuildMealsForCalories(List<Food> pool, double targetCal, Random rng)
    {
        if (pool.Count == 0) return new();

        var shuffled = pool.OrderBy(_ => rng.Next()).ToList();
        var entries = new List<MealEntry>();
        double remaining = targetCal;
        int maxItems = Math.Min(2, shuffled.Count);

        for (int i = 0; i < maxItems && remaining > 20; i++)
        {
            var food = shuffled[i];
            // calculate grams needed to hit remaining/maxItems calories
            double portionCal = remaining / (maxItems - i);
            double grams = Math.Round((portionCal / food.CaloriesPer100g) * 100, 0);
            grams = Math.Clamp(grams, 50, 400);

            var entry = BuildMealEntry(food, grams);
            entries.Add(entry);
            remaining -= entry.Calories;
        }

        return entries;
    }
}
