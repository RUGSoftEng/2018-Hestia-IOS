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

        public string ActivatorId { get; set; }
        public int Rank { get; set; }
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
        public String Name { get; set; }
        public Device Device { get; set; }
        public NetworkHandler Handler { get; set; }

        public Activator(String activatorId, int rank, String name)
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