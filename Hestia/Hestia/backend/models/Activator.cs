using Hestia.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Remoting;
using Hestia.backend.exceptions;

namespace Hestia.backend.models
{
    public class Activator
    {
        private string activatorId;
        private string name;
        private int rank;
        private ActivatorState state;
        private Device device;

        public string ActivatorId
        {
            get
            {
                return activatorId;
            }
            set
            {
                activatorId = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public int Rank
        {
            get
            {
                return rank;
            }
            set
            {
                rank = value;
            }
        }
        public ActivatorState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;

                string endpoint = strings.devicePath + device.DeviceId + "/" + strings.activatorsPath + activatorId;
                JObject activatorState = new JObject
                {
                    ["state"] = new JValue(state.RawState)
                };

                try
                {
                    Device.NetworkHandler.Post(activatorState, endpoint);
                }
                catch(ServerInteractionException ex)
                {
                    throw;
                }

            }
        }
        public Device Device
        {
            get
            {
                return device;
            }
            set
            {
                device = value;
            }
        }

        public Activator(string activatorId, string name, int rank, ActivatorState state)
        {
            this.activatorId = activatorId;
            this.name = name;
            this.rank = rank;
            this.state = state;
        }

        new
        public Boolean Equals(Object obj)
        {
            if (!(obj is Activator))
            {
                return false;
            }
            Activator activator = (Activator)obj;
            return (this == activator || (this.ActivatorId.Equals(activator.ActivatorId) &&
                rank.Equals(activator.Rank) &&
                state.Equals(activator.State) &&
                name.Equals(activator.Name)));
        }

        new
        public Exception GetHashCode()
        {
            return new NotImplementedException();
        }

        new
        public String ToString()
        {
            return this.name + " " + this.state;
        }
    }
}