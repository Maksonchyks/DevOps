using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutritionService.Data;
using NutritionService.Models;

namespace NutritionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodsController : ControllerBase
{
    private readonly NutritionDbContext _db;

    public FoodsController(NutritionDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.Foods.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var food = await _db.Foods.FindAsync(id);
        return food is null ? NotFound() : Ok(food);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Food food)
    {
        _db.Foods.Add(food);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = food.Id }, food);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Food updated)
    {
        var food = await _db.Foods.FindAsync(id);
        if (food is null) return NotFound();

        food.Name             = updated.Name;
        food.CaloriesPer100g  = updated.CaloriesPer100g;
        food.ProteinPer100g   = updated.ProteinPer100g;
        food.CarbsPer100g     = updated.CarbsPer100g;
        food.FatPer100g       = updated.FatPer100g;
        food.Category         = updated.Category;

        await _db.SaveChangesAsync();
        return Ok(food);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var food = await _db.Foods.FindAsync(id);
        if (food is null) return NotFound();
        _db.Foods.Remove(food);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
