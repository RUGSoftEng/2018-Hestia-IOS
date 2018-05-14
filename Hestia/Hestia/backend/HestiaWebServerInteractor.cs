using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using UIKit;

namespace Hestia.backend
{
    public class HestiaWebServerInteractor
    {
        private NetworkHandler networkHandler;

        public HestiaWebServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public HestiaServer GetServer(string serverId)
        {
            string endpoint = strings.serversPath + serverId;
            JToken payload = networkHandler.Get(endpoint);

            HestiaServerDeserializer deserializer = new HestiaServerDeserializer();
            HestiaServer server = deserializer.DeserializeServer(payload, networkHandler);

            return server;
        }

        public List<HestiaServer> GetServers()
        {
            string endpoint = strings.serversPath;
            JToken payload = networkHandler.Get(endpoint);

            HestiaServerDeserializer deserializer = new HestiaServerDeserializer();
            List<HestiaServer> servers = deserializer.DeserializeServers(payload, networkHandler);

            return servers;
        }
    }
}