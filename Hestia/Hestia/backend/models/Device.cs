using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

using Hestia.Resources;

namespace Hestia.backend.models
{
    public class Device
    {
        private string deviceId;
        private string name;
        private string type;
        private List<Activator> activators;
        private NetworkHandler networkHandler;

        public string DeviceId { get; set; }
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
                JObject deviceNameJson = new JObject
                {
                    ["name"] = name
                };

                JToken payload = networkHandler.Put(deviceNameJson, endpoint);
                if (payload["error"] != null)
                {
                    throw new ServerException();
                }
            }
        }
        public string Type { get; set; }
        public List<Activator> Activators { get; set; }
        public NetworkHandler NetworkHandler
        {
            get
            {
                return networkHandler;
            }
            set
            {
                networkHandler = value;
                foreach (Activator activator in activators)
                {
                    activator.Handler = value;
                }
            }
        }
        
        public Device(string deviceId, string name, string type, List<Activator> activator, NetworkHandler networkHandler)
        {
            this.deviceId = deviceId;
            this.name = name;
            this.type = type;
            this.activators = activator;
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
