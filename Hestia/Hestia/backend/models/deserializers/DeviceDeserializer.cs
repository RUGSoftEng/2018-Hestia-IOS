using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.backend.models.deserializers
{
    /// <summary>
    /// Class for deserializing a json object into a Device object or List of Device objects.
    /// </summary>
    public class DeviceDeserializer
    {
        /// <summary>
        /// Deserializes a json object into a Device object.
        /// </summary>
        /// <param name="jT"></param>
        /// <param name="serverInteractor"></param>
        /// <returns>A Device Object</returns>
        public Device DeserializeDevice(JToken jT, HestiaServerInteractor serverInteractor)
        {
            // get id, name and type
            string id = jT.Value<string>("deviceId");
            string name = jT.Value<string>("name");
            string type = jT.Value<string>("type");

            // get activators
            JToken activators = jT.SelectToken("activators");
            List<Activator> activatorList = new List<Activator>();
            ActivatorDeserializer activatorDeserializer = new ActivatorDeserializer();

            foreach(JToken activator in activators)
            {
                activatorList.Add(activatorDeserializer.DeserializeActivator(activator));
            }

            Device device = new Device(id, name, type, activatorList, serverInteractor);

            foreach(Activator activator in activatorList)
            {
                activator.Device = device;
            }

            return device;
        }

        /// <summary>
        /// Deserializes a json object into a list of Device objects.
        /// </summary>
        /// <param name="devices"></param>
        /// <param name="serverInteractor"></param>
        /// <returns>A list of Device objects</returns>
        public List<Device> DeserializeDevices(JToken devices, HestiaServerInteractor serverInteractor)
        {
            List<Device> deviceList = new List<Device>();
            
            foreach(JToken device in devices)
            {
                deviceList.Add(DeserializeDevice(device, serverInteractor));
            }

            return deviceList;
        }
    }
}
