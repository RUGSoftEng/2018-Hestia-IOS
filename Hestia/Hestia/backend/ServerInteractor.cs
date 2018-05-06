using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;

using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Hestia.Resources;

namespace Hestia.backend
{
    public class ServerInteractor
    {
        private NetworkHandler networkHandler;
      
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

        public ServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public List<Device> GetDevices()
        {
            JToken payload = null;

            try
            {
                payload = networkHandler.Get(strings.devicePath);
            }
            catch(ServerInteractionException ex)
            {
                throw ex;
            }

            DeviceDeserializer deserializer = new DeviceDeserializer();
            var devices = deserializer.DeserializeDevices(payload, networkHandler);

            return devices;
        }

        public void AddDevice(PluginInfo info)
        {
            JObject deviceInfo = JObject.FromObject(info);

            try
            {
                networkHandler.Post(deviceInfo, strings.devicePath);
            }
            catch(ServerInteractionException)
            {
                throw;
            }
        }

        public void RemoveDevice(Device device)
        {
            string endpoint = strings.devicePath + device.DeviceId;

            try
            {
                networkHandler.Delete(endpoint);
            }
            catch(ServerInteractionException)
            {
                throw;
            }
        }

        public List<string> GetCollections()
        {
            JToken payload = null;

            try
            {
                payload = networkHandler.Get(strings.pluginsPath);
            }
            catch(ServerInteractionException)
            {
                throw;
            } 

            List<string> collections = payload.ToObject<List<string>>();
            return collections;
        }

        public List<string> GetPlugins(string collection)
        {
            string endpoint = strings.pluginsPath + collection + "/";
            JToken payload = null;

            try
            {
                payload = networkHandler.Get(endpoint);
            }
            catch(ServerInteractionException)
            {
                throw;
            }

            List<string> plugins = payload.ToObject<List<string>>();
            return plugins;
        }

        public PluginInfo GetPluginInfo(string collection, string plugin)
        {
            string pluginsPath = strings.pluginsPath;
            string endpoint = pluginsPath + collection + "/" + pluginsPath + plugin;
            JToken payload = null;

            try
            {
                payload = networkHandler.Get(endpoint);
            }
            catch(ServerInteractionException)
            {
                throw;
            }

            PluginInfo info = payload.ToObject<PluginInfo>();
            return info;
        }

        public Dictionary<string, string> GetRequiredPluginInfo(string collection, string plugin)
        {
            PluginInfo info = GetPluginInfo(collection, plugin);
            return info.RequiredInfo;
        }

        override
        public string ToString()
        {
            return networkHandler.ToString();
        }
    }
}
