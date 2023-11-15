namespace CoreliaTask.SignalR
{
    public interface INotifyHub
    {
        Task sendnotification(string message);
    }
}
