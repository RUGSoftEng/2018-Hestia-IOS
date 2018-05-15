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
        private string hestiaWebEndpoint;

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
            this.hestiaWebEndpoint = strings.serversPath + serverId + '/' + strings.requestPath;
        }

        public List<Device> GetDevices()
        {
            JToken responsePayload = null;
            string endpoint = strings.devicePath;

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "GET" },
                    { "endpoint", '/' + endpoint },
                    { "optionalPayload", "{}" }
                };
                responsePayload = networkHandler.Post(requestPayload, hestiaWebEndpoint);
            } else
            {
                responsePayload = networkHandler.Get(endpoint);
            }

            DeviceDeserializer deserializer = new DeviceDeserializer();
            var devices = deserializer.DeserializeDevices(responsePayload, networkHandler);

            return devices;
        }

        public void AddDevice(PluginInfo info)
        {
            JToken responsePayload = null;
            JObject deviceInfo = JObject.FromObject(info);
            string endpoint = strings.devicePath;

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "POST" },
                    { "endpoint", '/' + endpoint },
                    { "optionalPayload", deviceInfo }
                };
                responsePayload = networkHandler.Post(requestPayload, hestiaWebEndpoint);
            }
            else
            {
                responsePayload = networkHandler.Post(deviceInfo, strings.devicePath);
            }
        }

        public void RemoveDevice(Device device)
        {
            string endpoint = strings.devicePath + device.DeviceId;

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "DELETE" },
                    { "endpoint", '/' + endpoint },
                    { "optionalPayload", "{}" }
                };
                networkHandler.Post(requestPayload, hestiaWebEndpoint);
            }
            else
            {
                networkHandler.Delete(endpoint);
            }
        }

        public List<string> GetCollections()
        {
            JToken responsePayload = null;
            string endpoint = strings.pluginsPath;

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "GET" },
                    { "endpoint", '/' + endpoint },
                    { "optionalPayload", "{}" }
                };
                responsePayload = networkHandler.Post(requestPayload, hestiaWebEndpoint);
            }
            else
            {
                responsePayload = networkHandler.Get(endpoint);
            }

            List<string> collections = responsePayload.ToObject<List<string>>();
            return collections;
        }

        public List<string> GetPlugins(string collection)
        {
            JToken responsePayload = null;
            string endpoint = strings.pluginsPath + collection + '/';

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "GET" },
                    { "endpoint", '/' + endpoint },
                    { "optionalPayload", "{}" }
                };
                responsePayload = networkHandler.Post(requestPayload, hestiaWebEndpoint);
            }
            else
            {
                responsePayload = networkHandler.Get(endpoint);
            }

            List<string> plugins = responsePayload.ToObject<List<string>>();
            return plugins;
        }

        public PluginInfo GetPluginInfo(string collection, string plugin)
        {
            JToken responsePayload = null;
            string endpoint = strings.pluginsPath + collection + "/" + strings.pluginsPath + plugin;

            if (isRemoteServer)
            {
                JObject requestPayload = new JObject
                {
                    { "requestType", "GET" },
                    { "endpoint", '/' + endpoint },
                    { "optionalPayload", "{}" }
                };
                responsePayload = networkHandler.Post(requestPayload, hestiaWebEndpoint);
            }
            else
            {
                responsePayload = networkHandler.Get(endpoint);
            }
            
            PluginInfo info = responsePayload.ToObject<PluginInfo>();
            return info;
        }

        public Dictionary<string, string> GetRequiredPluginInfo(string collection, string plugin)
        {
            PluginInfo info = GetPluginInfo(collection, plugin);
            return info.RequiredInfo;
        }

        public override string ToString()
        {
            return networkHandler.ToString();
        }
    }
}
