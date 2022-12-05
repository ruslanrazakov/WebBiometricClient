namespace MessengerWeb.Client.Services
{
    public class EngineEntity
    {
        public Engine Engine { get; set; }
        public string Name { get; set; }
        public string UUID { get; set; }
        public bool IsSelected { get; set; }
    }

    public enum Engine
    {
        Luna,
        Ntech,
        Tevian
    }
}
