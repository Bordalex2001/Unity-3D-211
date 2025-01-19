namespace GameEvents
{
    public interface INotifier { string message { get; } }

    public class MessageEvent : INotifier
    {
        public string message { get; set; }
        public object data { get; set; }
    }
}