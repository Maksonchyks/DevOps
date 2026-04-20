using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Hubs;

public class MealHub : Hub
{
    public async Task JoinUserGroup(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        await Clients.Caller.SendAsync("Joined", $"Connected to meal reminders for user {userId}");
    }

    public async Task LeaveUserGroup(string userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
    }
}
