using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Globalization;
using System.Json;
using System.Text;

namespace Hestia.backend.models
{
    public class Device
    {
        private String deviceId;
        private String name;
        private String type;
        private List<Activator> activators;
        private NetworkHandler handler;

        public String DeviceId { get; set; }
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                String endpoint = new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("devicePath") + deviceId;
                JsonObject obj = new JsonObject
                {
                    { "name", name }
                };
                /*JsonElement payload = handler.PUT(obj, endpoint);

                if (payload != null && payload.isJsonObject()) 
                {
                    JsonObject payloadObject = payload.getAsJsonObject();

                    if (payloadObject.has("error")) 
                    {
                        GsonBuilder gsonBuilder = new GsonBuilder();
                        Gson gson = gsonBuilder.create();
                        ComFaultException comFaultException = gson.fromJson(payload, ComFaultException.class);
                        throw comFaultException;

                    }
            }*/
                this.name = value;
            }
        }
        public String Type { get; set; }
        public List<Activator> Activators { get; set; }
        public NetworkHandler Handler
        {
            get
            {
                return handler;
            }
            set
            {
                this.handler = value;
                foreach (Activator activator in activators)
                {
                    activator.Handler = value;
                }
            }
        }

        public Device(String deviceId, String name, String type, List<Activator> activator, NetworkHandler handler)
        {
            this.deviceId = deviceId;
            this.name = name;
            this.type = type;
            this.activators = activator;
            this.handler = handler;
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
