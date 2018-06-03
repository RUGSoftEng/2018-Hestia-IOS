
namespace Hestia.backend.models
{
    public class Activator
    {
        string activatorId;
        string name;
        int rank;
        ActivatorState state;
        Device device;

        public string ActivatorId
        {
            get => activatorId;
            set => activatorId = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }
        public int Rank
        {
            get => rank;
            set => rank = value;
        }
        public ActivatorState State
        {
            get => state;
            set
            {
                state = value;
                device.ServerInteractor.SetActivatorState(this, state);
            }
        }
        public Device Device
        {
            get => device;
            set => device = value;
        }

        public Activator(string activatorId, string name, int rank, ActivatorState state)
        {
            this.activatorId = activatorId;
            this.name = name;
            this.rank = rank;
            this.state = state;
        }

        public override string ToString()
        {
            return name + " " + state;
        }
    }
}
