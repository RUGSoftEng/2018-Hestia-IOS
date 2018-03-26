using System;

namespace Hestia.backend.models
{
    class Activator
    {
        private string activatorId;
        private string name;
        private int rank;
        private ActivatorState<Object> state;
        private Device device;
        private NetworkHandler networkHandler;

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
        public ActivatorState<Object> State
        {
            get
            {
                return state;
            }
            set
            {
                this.state = value;
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
        public NetworkHandler Handler
        {
            get
            {
                return networkHandler;
            }
            set
            {
                networkHandler = value;
            }
        }

        public Activator(string activatorId, int rank, string name)
        {
            this.activatorId = activatorId;
            this.rank = rank;
            this.name = name;
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
                this.Rank.Equals(activator.Rank) &&
                this.State.Equals(activator.State) &&
                this.Name.Equals(activator.Name) &&
                this.Handler.Equals(activator.Handler)));
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