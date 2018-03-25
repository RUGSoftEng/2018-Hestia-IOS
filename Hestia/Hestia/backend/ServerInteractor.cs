using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;

using Hestia.backend.models;
using Newtonsoft.Json;
using System;

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

        public void AddDevice(RequiredInfo info)
        {
            JObject deviceInfo = JObject.FromObject(info);
            JToken payload = networkHandler.Post(deviceInfo, "devices/");

            if(payload["error"] != null)
            {
                throw new ServerException();
            }
        }

        public void RemoveDevice(Device device)
        {
            string endpoint = "devices/" + device.DeviceId;
            JToken payload = networkHandler.Delete(endpoint);

            if(payload["error"] != null)
            {
                throw new ServerException();
            }
        }
    }
}
