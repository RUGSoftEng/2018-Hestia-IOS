using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.Remoting;

namespace Hestia.backend.models
{
    class Device
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

                string endpoint = "devices/" + deviceId;
                JObject jsonName = new JObject
                {
                    ["name"] = name
                };

                JToken payload = networkHandler.Put(jsonName, endpoint);
                if (payload["error"] != null)
                {
                    // Throwing a default exception for now, a custom exception should be made later on.
                    throw new ServerException();
                }
            }
        }
        public string Type { get; set; }
        public List<Activator> Activators { get; set; }
        public NetworkHandler Handler
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
                    this.Name.Equals(device.Name) &&
                    this.Type.Equals(device.Type) &&
                    this.Activators.Equals(device.Activators) &&
                    this.Handler.Equals(device.Handler)));
        }

        new
        public int GetHashCode()
        {
            int multiplier = int.Parse(new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("hashCodeMultiplier"));
            int result = DeviceId.GetHashCode();
            result = result * multiplier + Name.GetHashCode();
            result = result * multiplier + GetType().GetHashCode();
            result = result * multiplier + Activators.GetHashCode();
            result = result * multiplier + Handler.GetHashCode();
            return result;
        }

        new
        public String ToString()
        {
            return name + " " + deviceId + " " + activators + "\n";
        }
    }
}
