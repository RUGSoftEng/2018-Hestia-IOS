using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;

using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using System.Resources;
using System.Reflection;

namespace Hestia.backend
{
    class ServerInteractor
    {
        private NetworkHandler networkHandler;

        public ServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public List<Device> GetDevices()
        {
            JToken payload = networkHandler.Get(
                new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("devicePath"));

            if(payload is JArray)
            {
                DeviceDeserializer deserializer = new DeviceDeserializer();
                var devices = deserializer.DeserializeDevices(payload, networkHandler);

                return devices;
            }
            else
            {
                throw new ServerException();
            }
        }

        public void AddDevice(RequiredInfo info)
        {
            JObject deviceInfo = JObject.FromObject(info);
            JToken payload = networkHandler.Post(deviceInfo,
                new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("devicePath"));

            if(payload["error"] != null)
            {
                throw new ServerException();
            }
        }

        public void RemoveDevice(Device device)
        {
            string endpoint = new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("devicePath") + device.DeviceId;
            JToken payload = networkHandler.Delete(endpoint);

            if(payload["error"] != null)
            {
                throw new ServerException();
            }
        }

        public List<string> GetCollections()
        {
            JToken payload = networkHandler.Get(new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("pluginsPath"));

            if(payload is JArray)
            {
                List<string> collections = payload.ToObject<List<string>>();
                return collections;
            } else
            {
                throw new ServerException();
            }
        }

        public List<string> GetPlugins(string collection)
        {
            string endpoint = new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("pluginsPath") + collection + "/";
            JToken payload = networkHandler.Get(endpoint);

            if (payload is JArray)
            {
                List<string> plugins = payload.ToObject<List<string>>();
                return plugins;
            }
            else
            {
                throw new ServerException();
            }
        }

        public RequiredInfo GetRequiredInfo(string collection, string plugin)
        {
            string temp = new ResourceManager("strings", Assembly.GetExecutingAssembly()).GetString("pluginsPath");
            string endpoint = temp + collection + "/" + temp + plugin;
            JToken payload = networkHandler.Get(endpoint);

            if(payload["error"] == null)
            {
                RequiredInfo info = payload.ToObject<RequiredInfo>();
                return info;
            }
            else
            {
                throw new ServerException();
            }
        }
    }
}
