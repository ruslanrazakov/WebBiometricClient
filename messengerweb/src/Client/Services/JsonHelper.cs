using MessengerWeb.Shared;
using System;
using System.Text.Json;

namespace MessengerWeb.Client.Services
{
    public class JsonHelper
    {
        public Person ConvertToPerson(string json)
        {
            if (json == String.Empty)
                return new Person();
            try
            {
                var person = JsonSerializer.Deserialize<Person>(json);
                return person;
            }
            catch { }
            return new Person();
        }
    }
}
