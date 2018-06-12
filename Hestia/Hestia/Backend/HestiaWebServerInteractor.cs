using Hestia.Backend.Models;
using Hestia.Backend.Models.Deserializers;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.Backend
{
    /// <summary>
    /// This class is a facade, performing the basic operations between the user and the hestia web team server.
    /// It does so using the NetworkHandler.
    /// </summary>
    public class HestiaWebServerInteractor
    {
        NetworkHandler networkHandler;

        public HestiaWebServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public NetworkHandler NetworkHandler
        {
            get => networkHandler;
            set => networkHandler = value;
        }

        /// <summary>
        /// This methods posts an user on the web team server.
        /// It should be called only once after the app launches.
        /// If the user already exists it will always throw an ServerInteractionException.
        /// </summary>
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
                { "server_port", port.ToString() }
            };            
            string endpoint = strings.serversPath;
            networkHandler.Post(payload, endpoint);
        }

        public void DeleteServer(HestiaServer server)
        {
            string endpoint = strings.serversPath + server.Id;
            networkHandler.Delete(endpoint);
        }

        public void EditServer(HestiaServer server, string name, string address, int port)
        {
            JObject payload = new JObject
            {
                { "server_name", name },
                { "server_address", address },
                { "server_port", port.ToString() }
            };
            string endpoint = strings.serversPath + server.Id;
            networkHandler.Put(payload, endpoint);
        }
    }
}
