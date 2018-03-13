using System;
using System.Collections.Generic;
using System.Json;
using System.Text;

namespace Hestia.backend.models
{
    class Device
    {
        private String deviceId;
        private String name;
        private String type;
        private List<Activator> activators;
        private NetworHandler handler;

        public Device(String deviceId, String name, String type, List<Activator> activator, NetworkHandler handler)
        {
            this.deviceId = deviceId;
            this.name = name;
            this.type = type;
            this.activators = activator;
            this.handler = handler;
        }

        public String GetId()
        {
            return deviceId;
        }

        public void SetId(String deviceId)
        {
            this.deviceId = deviceId;
        }

        public void SetType(String type)
        {
            this.type = type;
        }

        public NetworkHandler GetHandler()
        {
            return handler;
        }

        public void SetHandler(NetworkHandler handler)
        {
            this.handler = handler;
            foreach (Activator activator in activators)
            {
                activator.SetHandler(handler);
            }
        }

        public String GetName()
        {
            return name;
        }

        public void SetName(String name)
        {
            /*
             * This code needs still needs transfering, 
                String endpoint = HestiaApplication.getContext().getString(R.string.devicePath) + deviceId;
                JsonObject obj = new JsonObject();
                obj.addProperty("name", name);
                JsonElement payload = handler.PUT(obj, endpoint);

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
            this.name = name;
        }

        public List<Activator> GetActivators()
        {
            return activators;
        }

        public void SetActivators(List<Activator> activators)
        {
            this.activators = activators;
        }

        new
        public Boolean Equals(Object obj)
        {
            if (!(obj is Device)) return false;
            Device device = (Device)obj;
            return (this == device || (this.GetId().Equals(device.GetId()) &&
                    this.GetName().Equals(device.GetName()) &&
                    this.GetType().Equals(device.GetType()) &&
                    this.GetActivators().Equals(device.GetActivators()) &&
                    this.GetHandler().equals(device.GetHandler())));
        }

        new
        public int GetHashCode()
        {
            int multiplier = 0; //int.Parse(HestiaApplication.getContext().getString(R.string.hashCodeMultiplier)); This can't be used yet.
            int result = GetId().GetHashCode();
            result = result * multiplier + GetName().GetHashCode();
            result = result * multiplier + GetType().GetHashCode();
            result = result * multiplier + GetActivators().GetHashCode();
            result = result * multiplier + GetHandler().HashCode();
            return result;
        }

        new
        public String ToString()
        {
            return name + " " + deviceId + " " + activators + "\n";
        }
    }
}
