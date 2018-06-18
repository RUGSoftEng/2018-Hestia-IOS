using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.Backend.Models.Deserializers
{
    /// <summary>
    /// Class for deserializing a json object into a HestiaServer object or List of HestiaServer objects.
    /// </summary>
    public class HestiaServerDeserializer
    {
        /// <summary>
        /// Deserializes a json object into a HestiaServer object.
        /// </summary>
        /// <param name="jsonServer"></param>
        /// <param name="networkHandler"></param>
        /// <returns>A HestiaServer object</returns>
        public HestiaServer DeserializeServer(JToken jsonServer, NetworkHandler networkHandler)
        {
            string id = jsonServer.Value<string>("server_id");
            string name = jsonServer.Value<string>("server_name");
            string address = jsonServer.Value<string>("server_address");
            int port = jsonServer.Value<int>("server_port");
            HestiaServerInteractor interactor = new HestiaServerInteractor(networkHandler, id);

            HestiaServer server = new HestiaServer(false, interactor)
            {
                Id = id,
                Name = name,
                Address = address,
                Port = port
            };

            return server;
        }

        /// <summary>
        /// Deserializes a json object into a list of HestiaServer objects.
        /// </summary>
        /// <param name="jsonServers"></param>
        /// <param name="networkHandler"></param>
        /// <returns>A list of HestiaServer objects</returns>
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
