using Microsoft.AspNetCore.SignalR;

namespace CoreliaTask.SignalR
{
    public class NotifyHub:Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceviveNotification", message);
        }
    }
}
