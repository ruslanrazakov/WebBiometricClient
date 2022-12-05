using System.Text.Json;

namespace MessengerWeb.Client.Constants
{
    public class Constants
    {
        public static JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };
    }
}
