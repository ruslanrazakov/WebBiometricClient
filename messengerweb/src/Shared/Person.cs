using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessengerWeb.Shared
{
    public class Person
    {
        public int Id { get; set; }
        [JsonPropertyName("uuid")]
        public string UUID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("position")]
        public string Position { get; set; }
    }
}
