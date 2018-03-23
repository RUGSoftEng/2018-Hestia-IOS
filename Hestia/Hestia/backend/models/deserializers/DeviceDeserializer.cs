using Newtonsoft.Json;

namespace Hestia.backend.models.deserialisers
{
    /**
     * Helper class that deserializes a json object into a Device object
     */
    class DeviceDeserializer
    {
        public DeviceDeserializer() { }

        public Device deserializeDevice(string jsonData)
        {
            return JsonConvert.DeserializeObject<Device>(jsonData);
        }
    }
}