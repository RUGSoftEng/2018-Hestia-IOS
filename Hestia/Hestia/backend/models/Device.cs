using System;
using System.Collections.Generic;

namespace Hestia.backend.models
{
    public class Device
    {
        private string deviceId;
        private string name;
        private string type;
        private List<Activator> activators;
        private HestiaServerInteractor serverInteractor;

        public string DeviceId
        {
            get => deviceId;
            set => deviceId = value;  
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                serverInteractor.ChangeDeviceName(this, name);
            }
        }
        public string Type
        {
            get => type;
            set => type = value;
        }
        public List<Activator> Activators
        {
            get => activators;
            set => activators = value;
        }
        public HestiaServerInteractor ServerInteractor
        {
            get => serverInteractor;
            set => serverInteractor = value;
        }
        
        public Device(string deviceId, string name, string type, List<Activator> activators, HestiaServerInteractor serverInteractor)
        {
            this.deviceId = deviceId;
            this.name = name;
            this.type = type;
            this.activators = activators;
            this.serverInteractor = serverInteractor;
        }

        public override string ToString()
        {
            return name + " " + deviceId;
        }
    }
}
