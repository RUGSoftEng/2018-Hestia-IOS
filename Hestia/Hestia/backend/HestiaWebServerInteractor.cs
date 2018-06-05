﻿using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.backend
{
    public class HestiaWebServerInteractor
    {
        private NetworkHandler networkHandler;

        public HestiaWebServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public NetworkHandler NetworkHandler
        {
            get => networkHandler;
            set => networkHandler = value;
        }

        // This method should be called before any other method in this class
        public void PostUser()
        {
            string endpoint = strings.usersPath;
            networkHandler.Post(null, endpoint);
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

        public void AddServer(string name, string address, int port) 
        {
            JObject payload = new JObject
            {
                { "server_name", name },
                { "server_address", address },
                { "server_port", port }
            };            
            string endpoint = strings.serversPath;
            networkHandler.Post(payload, endpoint);
        }
    }
}
