using Microsoft.AspNetCore.SignalR;

namespace CoreliaTask.SignalR
{
    public class NotifyHub:Hub<INotifyHub>
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.sendnotification(message);
        }
    }
}
