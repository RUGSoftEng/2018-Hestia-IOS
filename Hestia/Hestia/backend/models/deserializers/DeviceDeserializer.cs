using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.backend.models.deserializers
{
    /**
     * Helper class that deserializes a jToken into a Device object
     */
    class DeviceDeserializer
    {

        // deserialize a single device from a JToken
        public Device deserialize(JToken jT, NetworkHandler handler)
        {
            // get id, name and type
            string id = jT.SelectToken("deviceId").ToString();
            string name = jT.SelectToken("name").ToString();
            string type = jT.SelectToken("type").ToString();

            // get activators
            JToken activators = jT.SelectToken("activators");
            List<Activator> activatorList = new List<Activator>();
            ActivatorDeserializer activatorDeserializer = new ActivatorDeserializer();

            foreach (JToken activator in activators)
            {
                activatorList.Add(activatorDeserializer.deserialize(activator));
            }

            return new Device(id, name, type, activatorList, handler);
        }

        // Use this function if you want to deserialize multiple devices.
        // Useful when you get all the devices from the server and want to deserialize them into a list of devices.
        public List<Device> deserializeDevices(JToken devices, NetworkHandler handler)
        {
            List<Device> deviceList = new List<Device>();

            foreach(JToken device in devices)
            {
                deviceList.Add(this.deserialize(device, handler));
            }

            return deviceList;
        }
    }
}