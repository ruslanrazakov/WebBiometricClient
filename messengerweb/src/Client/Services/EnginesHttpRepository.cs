using MessengerWeb.Shared;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MessengerWeb.Client.Services
{
    public class EnginesHttpRepository
    {
        private readonly HttpClient http;
        public EnginesHttpRepository(HttpClient http)
        {
            this.http = http;
        }

        public async Task<string> Post(string imageBytes, string url, string engineId)
        {
            var data = Convert.FromBase64String(imageBytes);
            var contents = new StreamContent(new MemoryStream(data));
            var form = new MultipartFormDataContent();
            form.Add(contents, "data", "image");
            var response = await http.PostAsync($"{url}/{engineId}", form);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SavePerson(Person person, string url)
        {
            await http.PostAsJsonAsync(url, person);
        }
    }
}