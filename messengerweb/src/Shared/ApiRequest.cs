using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MessengerWeb.Shared
{
    public class ApiRequest
    {
        [JsonPropertyName("frames")]
        public List<byte[]> Frames { get; set; }
    }
}
