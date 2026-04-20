using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutritionService.Data;
using NutritionService.Models;
using NutritionService.Services;

namespace NutritionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealPlanController : ControllerBase
{
    private readonly NutritionDbContext _db;
    private readonly IMealPlannerService _planner;

    public MealPlanController(NutritionDbContext db, IMealPlannerService planner)
    {
        _db = db;
        _planner = planner;
    }

    /// <summary>
    /// Generate a meal plan based on calorie target, days count, and goal.
    /// Goal: lose | gain | maintain
    /// </summary>
    [HttpPost("generate")]
    public async Task<IActionResult> Generate(MealPlanRequest request)
    {
        if (request.Days < 1 || request.Days > 7)
            return BadRequest("Days must be between 1 and 7.");
        if (request.TargetCalories < 800 || request.TargetCalories > 5000)
            return BadRequest("TargetCalories must be between 800 and 5000.");

        var foods = await _db.Foods.ToListAsync();
        if (foods.Count == 0)
            return BadRequest("No foods in database. Seed the database first.");

        var plan = _planner.GeneratePlan(foods, request);
        return Ok(plan);
    }
}
