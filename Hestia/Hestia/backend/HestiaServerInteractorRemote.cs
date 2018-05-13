using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hestia.backend
{
    public class HestiaServerInteractorRemote : IHestiaServerInteractor
    {
        private NetworkHandler networkHandler;
        private string serverId;

        public NetworkHandler NetworkHandler
        {
            get => networkHandler;
            set => networkHandler = value;
        }

        public HestiaServerInteractorRemote(NetworkHandler networkHandler, string serverId)
        {
            this.networkHandler = networkHandler;
            this.serverId = serverId;
        }

        public List<Device> GetDevices()
        {
            string endpoint = strings.auth0ServersPath + serverId + '/' + strings.auth0RequestPath;
            JToken payload = networkHandler.Get(endpoint);

            DeviceDeserializer deserializer = new DeviceDeserializer();
            var devices = deserializer.DeserializeDevices(payload, networkHandler);

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

        public List<string> GetServers()
        {
            string endpoint = strings.auth0ServersPath + '/';
            JToken payload = networkHandler.Get(endpoint);

            return new List<string>();
        }

        override
        public string ToString()
        {
            return networkHandler.ToString();
        }
    }
}
