using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;

using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Hestia.Resources;

namespace Hestia.backend
{
    public class ServerInteractor
    {
        private NetworkHandler networkHandler;

        public ServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public List<Device> GetDevices()
        {
            JToken payload = networkHandler.Get(strings.devicePath);

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

        public void AddDevice(PluginInfo info)
        {
            JObject deviceInfo = JObject.FromObject(info);
            JToken payload = networkHandler.Post(deviceInfo, strings.devicePath);

            if(payload["error"] != null)
            {
                throw new ServerException();
            }
        }

        public void RemoveDevice(Device device)
        {
            string endpoint = strings.devicePath + device.DeviceId;
            JToken payload = networkHandler.Delete(endpoint);

            if(payload["error"] != null)
            {
                throw new ServerException();
            }
        }

        public List<string> GetCollections()
        {
            JToken payload = networkHandler.Get(strings.pluginsPath);

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
            string endpoint = strings.pluginsPath + collection + "/";
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

        public PluginInfo GetPluginInfo(string collection, string plugin)
        {
            string pluginsPath = strings.pluginsPath;
            string endpoint = pluginsPath + collection + "/" + pluginsPath + plugin;
            JToken payload = networkHandler.Get(endpoint);

            if(payload["error"] == null)
            {
                PluginInfo info = payload.ToObject<PluginInfo>();
                return info;
            }
            else
            {
                throw new ServerException();
            }
        }

        public Dictionary<string, string> GetRequiredPluginInfo(string collection, string plugin)
        {
            PluginInfo info = GetPluginInfo(collection, plugin);
            return info.RequiredInfo;
        }
    }
}
