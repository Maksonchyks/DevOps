using Microsoft.EntityFrameworkCore;
using NutritionService.Data;
using NutritionService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=nutriplan;Username=postgres;Password=postgres";

builder.Services.AddDbContext<NutritionDbContext>(opt =>
    opt.UseNpgsql(connectionString));

builder.Services.AddScoped<IMealPlannerService, MealPlannerService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "NutriPlan – Nutrition Service", Version = "v1" });
});

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NutritionDbContext>();

        System.Threading.Thread.Sleep(2000);
        context.Database.Migrate();
        Console.WriteLine("----> Database migrated successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"----> Migration Error: {ex.Message}");
    }
}



app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

public partial class Program { } // needed for integration tests
