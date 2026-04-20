using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Services;

public class ReminderScheduler : BackgroundService
{
    private readonly IHubContext<MealHub> _hub;
    private readonly ILogger<ReminderScheduler> _logger;

    // Meal times: breakfast 8:00, lunch 13:00, dinner 19:00, snack 16:00
    private static readonly (TimeOnly time, string meal)[] Schedule =
    {
        (new TimeOnly(8,  0), "Сніданок 🌅"),
        (new TimeOnly(13, 0), "Обід 🥗"),
        (new TimeOnly(16, 0), "Перекус 🍎"),
        (new TimeOnly(19, 0), "Вечеря 🍽️"),
    };

    public ReminderScheduler(IHubContext<MealHub> hub, ILogger<ReminderScheduler> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ReminderScheduler started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = TimeOnly.FromDateTime(DateTime.Now);

            foreach (var (mealTime, mealName) in Schedule)
            {
                // fire within a 1-minute window of the scheduled time
                if (Math.Abs(now.ToTimeSpan().TotalMinutes - mealTime.ToTimeSpan().TotalMinutes) < 1)
                {
                    var message = $"Час на {mealName}! Не забудьте дотриматись плану харчування.";
                    _logger.LogInformation("Broadcasting reminder: {Meal}", mealName);
                    await _hub.Clients.All.SendAsync("MealReminder", new
                    {
                        meal = mealName,
                        time = mealTime.ToString("HH:mm"),
                        message
                    }, stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
