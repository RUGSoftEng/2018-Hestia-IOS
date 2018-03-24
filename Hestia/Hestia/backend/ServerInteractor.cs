using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;

using Hestia.backend.models;

namespace Hestia.backend
{
    class ServerInteractor
    {
        private NetworkHandler networkHandler;

        public ServerInteractor(NetworkHandler networkHandler)
        {
            this.networkHandler = networkHandler;
        }

        public List<Device> GetDevices()
        {
            JToken payload = networkHandler.Get("devices/");

            if(payload is JArray)
            {
                var devices = payload.ToObject<List<Device>>();
                return devices;
            }
            else
            {
                throw new ServerException();
            }
        }
    }
}