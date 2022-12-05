using System.Collections.Generic;

namespace MessengerWeb.Client.Services
{
    public class Settings
    {
        public List<EngineEntity> Engines { get; set; }

        public Settings()
        {
            Engines = new List<EngineEntity> 
            {
                new EngineEntity()
                {
                    Engine = Engine.Luna,
                    Name = "Luna Platform",
                    UUID = "315c9de1-317d-4f78-bc4b-e10896e48e6c",
                    IsSelected = true,
                },
                new EngineEntity()
                {
                    Engine = Engine.Ntech,
                    Name = "NTech Find Face",
                    UUID = "d4f26c41-f080-4321-8d69-26cf06db1bc9",
                    IsSelected = false,
                },
                new EngineEntity()
                {
                    Engine = Engine.Tevian,
                    Name = "Tevian Platform",
                    UUID = "a720a37c-89bf-4ce2-8dab-90f4ac4dd5f1",
                    IsSelected = false,
                },
            };
        }
    }
}
