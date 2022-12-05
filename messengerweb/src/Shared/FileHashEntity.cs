using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessengerWeb.Shared
{
    public class FileHashEntity
    {
        [JsonPropertyName("file_hash")]
        public string Hash { get; set; }
        [JsonPropertyName("exp")]
        public int ExpireDate { get; set; }
    }
}
