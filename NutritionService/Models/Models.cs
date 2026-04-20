namespace NutritionService.Models;

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double CaloriesPer100g { get; set; }
    public double ProteinPer100g { get; set; }
    public double CarbsPer100g { get; set; }
    public double FatPer100g { get; set; }
    public string Category { get; set; } = ""; // breakfast, lunch, dinner, snack
}

public class MealPlanRequest
{
    public int TargetCalories { get; set; } = 2000;
    public int Days { get; set; } = 1;
    public string Goal { get; set; } = "maintain"; // lose, gain, maintain
}

public class MealEntry
{
    public string FoodName { get; set; } = "";
    public double GramsAmount { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
}

public class DayPlan
{
    public int DayNumber { get; set; }
    public List<MealEntry> Breakfast { get; set; } = new();
    public List<MealEntry> Lunch { get; set; } = new();
    public List<MealEntry> Dinner { get; set; } = new();
    public List<MealEntry> Snacks { get; set; } = new();
    public double TotalCalories { get; set; }
    public double TotalProtein { get; set; }
    public double TotalCarbs { get; set; }
    public double TotalFat { get; set; }
}

public class MealPlan
{
    public List<DayPlan> Days { get; set; } = new();
    public int TargetCalories { get; set; }
    public string Goal { get; set; } = "";
}
