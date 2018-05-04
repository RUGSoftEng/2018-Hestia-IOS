using Hestia.backend.exceptions;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Hestia.backend.models
{
    public class Device
    {
        private string deviceId;
        private string name;
        private string type;
        private List<Activator> activators;
        private NetworkHandler networkHandler;

        public string DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
            }   
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;

                string endpoint = strings.devicePath + deviceId;
                JObject deviceName = new JObject
                {
                    ["name"] = name
                };

                try
                {
                    networkHandler.Put(deviceName, endpoint);
                }
                catch (ServerInteractionException ex)
                {
                    throw;
                }
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        public List<Activator> Activators
        {
            get
            {
                return activators;
            }
            set
            {
                activators = value;
            }
        }
        public NetworkHandler NetworkHandler
        {
            get
            {
                return networkHandler;
            }
            set
            {
                networkHandler = value;
            }
        }
        
        public Device(string deviceId, string name, string type, List<Activator> activators, NetworkHandler networkHandler)
        {
            this.deviceId = deviceId;
            this.name = name;
            this.type = type;
            this.activators = activators;
            this.networkHandler = networkHandler;
        }

        new
        public Boolean Equals(Object obj)
        {
            if (!(obj is Device)) return false;
            Device device = (Device)obj;
            return (this == device || (this.DeviceId.Equals(device.DeviceId) &&
                    Name.Equals(device.Name) &&
                    Type.Equals(device.Type) &&
                    Activators.Equals(device.Activators) &&
                    NetworkHandler.Equals(device.NetworkHandler)));
        }

        new
        public int GetHashCode()
        {
            int multiplier = int.Parse(Resources.strings.hashCodeMultiplier);
            int result = DeviceId.GetHashCode();
            result = result * multiplier + Name.GetHashCode();
            result = result * multiplier + GetType().GetHashCode();
            result = result * multiplier + Activators.GetHashCode();
            result = result * multiplier + NetworkHandler.GetHashCode();
            return result;
        }

        new
        public String ToString()
        {
            return name + " " + deviceId + " " + activators + "\n";
        }
    }
}
