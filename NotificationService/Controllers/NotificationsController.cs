using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IHubContext<MealHub> _hub;

    public NotificationsController(IHubContext<MealHub> hub) => _hub = hub;

    /// <summary>
    /// Send a custom meal reminder to all connected clients.
    /// </summary>
    [HttpPost("broadcast")]
    public async Task<IActionResult> Broadcast([FromBody] BroadcastRequest req)
    {
        await _hub.Clients.All.SendAsync("MealReminder", new
        {
            meal = req.Meal,
            time = DateTime.Now.ToString("HH:mm"),
            message = req.Message
        });
        return Ok(new { sent = true });
    }

    /// <summary>
    /// Send a reminder to a specific user group.
    /// </summary>
    [HttpPost("user/{userId}")]
    public async Task<IActionResult> NotifyUser(string userId, [FromBody] BroadcastRequest req)
    {
        await _hub.Clients.Group($"user_{userId}").SendAsync("MealReminder", new
        {
            meal = req.Meal,
            time = DateTime.Now.ToString("HH:mm"),
            message = req.Message
        });
        return Ok(new { sent = true, userId });
    }
}

public record BroadcastRequest(string Meal, string Message);
