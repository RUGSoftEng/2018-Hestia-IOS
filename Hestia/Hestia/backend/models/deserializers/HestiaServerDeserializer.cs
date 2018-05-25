using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;

namespace Hestia.backend.models.deserializers
{
    public class HestiaServerDeserializer
    {
        public HestiaServer DeserializeServer(JToken jsonServer, NetworkHandler networkHandler)
        {
            string id = jsonServer.Value<string>("server_id");
            string name = jsonServer.Value<string>("server_name");
            string ip = jsonServer.Value<string>("server_address");
            int port = jsonServer.Value<int>("server_port");
            HestiaServerInteractor interactor = new HestiaServerInteractor(networkHandler, id);

            HestiaServer server = new HestiaServer(false, interactor)
            {
                Id = id,
                Name = name,
                Ip = ip,
                Port = port
            };

            return server;
        }

        public List<HestiaServer> DeserializeServers(JToken jsonServers, NetworkHandler networkHandler)
        {
            List<HestiaServer> servers = new List<HestiaServer>();

            foreach(JToken jsonServer in jsonServers)
            {
                servers.Add(DeserializeServer(jsonServer, networkHandler));
            }

            return servers;
        }
    }
}
