using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.backend
{
    public class HestiaServerInteractor
    {
        private NetworkHandler networkHandler;
        private bool isRemoteServer;
        private string serverId;

        public NetworkHandler NetworkHandler
        {
            get => networkHandler;
            set => networkHandler = value;
        }
        public bool IsRemoteServer
        {
            get => isRemoteServer;
        }

        public HestiaServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
            this.isRemoteServer = false;
        }

        public HestiaServerInteractor(NetworkHandler networkHandler, string serverId)
        {
            this.networkHandler = networkHandler;
            this.serverId = serverId;
            this.isRemoteServer = true;
        }

        public List<Device> GetDevices()
        {
            JToken responsePayload = null;

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "GET" },
                    { "endpoint", '/' + strings.devicePath },
                    { "optionalPayload", null }
                };
                string endpoint = strings.serversPath + serverId + '/' + strings.requestPath;
                responsePayload = networkHandler.Post(requestPayload, endpoint);
            } else
            {
                responsePayload = networkHandler.Get(strings.devicePath);
            }

            DeviceDeserializer deserializer = new DeviceDeserializer();
            var devices = deserializer.DeserializeDevices(responsePayload, networkHandler);

            return devices;
        }

        public void AddDevice(PluginInfo info)
        {
            JObject deviceInfo = JObject.FromObject(info);
            networkHandler.Post(deviceInfo, strings.devicePath);
        }

        public void RemoveDevice(Device device)
        {
            string endpoint = strings.devicePath + device.DeviceId;
            networkHandler.Delete(endpoint);
        }

        public List<string> GetCollections()
        {
            JToken payload = networkHandler.Get(strings.pluginsPath);
            
            List<string> collections = payload.ToObject<List<string>>();
            return collections;
        }

        public List<string> GetPlugins(string collection)
        {
            string endpoint = strings.pluginsPath + collection + "/";
            JToken payload = networkHandler.Get(endpoint);

            List<string> plugins = payload.ToObject<List<string>>();
            return plugins;
        }

        public PluginInfo GetPluginInfo(string collection, string plugin)
        {
            string pluginsPath = strings.pluginsPath;
            string endpoint = pluginsPath + collection + "/" + pluginsPath + plugin;
            JToken payload = networkHandler.Get(endpoint);
            
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
