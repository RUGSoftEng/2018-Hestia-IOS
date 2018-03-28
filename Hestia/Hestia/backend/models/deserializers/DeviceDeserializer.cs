﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Hestia.backend.models.deserializers
{
    /**
     * Helper class that deserializes a jToken into a Device object
     */
    public class DeviceDeserializer
    {
        // deserialize a single device from a JToken
        public Device Deserialize(JToken jT, NetworkHandler networkHandler)
        {
            // get id, name and type
            string id = jT.Value<string>("deviceId");
            string name = jT.Value<string>("name");
            string type = jT.Value<string>("name");

            // get activators
            JToken activators = jT.SelectToken("activators");
            List<Activator> activatorList = new List<Activator>();
            ActivatorDeserializer activatorDeserializer = new ActivatorDeserializer();

            foreach(JToken activator in activators)
            {
                activatorList.Add(activatorDeserializer.Deserialize(activator));
            }

            Device device = new Device(id, name, type, activatorList, networkHandler);

            foreach(Activator activator in activatorList)
            {
                activator.Device = device;
            }

            return device;
        }

        // Use this function if you want to deserialize multiple devices.
        // Useful when you get all the devices from the server and want to deserialize them into a list of devices.
        public List<Device> DeserializeDevices(JToken devices, NetworkHandler networkHandler)
        {
            List<Device> deviceList = new List<Device>();
            
            foreach(JToken device in devices)
            {
                deviceList.Add(Deserialize(device, networkHandler));
            }

            return deviceList;
        }
    }
}