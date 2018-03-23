using Newtonsoft.Json;

namespace Hestia.backend.models.deserialisers
{
    /**
     * Helper class that deserializes a json object into an Activator object
     */
    class ActivatorDeserializer
    {
        public ActivatorDeserializer() { }

        public Activator deserializeJson(string jsonData)
        {
            return JsonConvert.DeserializeObject<Activator>(jsonData);
        }

    }
}
