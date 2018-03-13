using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend.models
{
    class Activator
    {
        private String activatorId;
        private int rank;
        private ActivatorState<Object> state;
        private String name;
        public Device device;
        private NetworkHandler handler;

        public Activator(String activatorId, int rank, String name)
        {
            this.activatorId = activatorId;
            this.rank = rank;
            this.name = name;
        }

        public String GetId()
        {
            return this.activatorId;
        }

        public void SetId(String activatorId)
        {
            this.activatorId = activatorId;
        }

        public int GetRank()
        {
            return rank;
        }

        public void SetRank(int rank)
        {
            this.rank = rank;
        }

        public ActivatorState<Object> GetState()
        {
            return this.state;
        }

        public void setState(ActivatorState<Object> state) 
        {
            throw new NotImplementedException();
        }

        public String GetName()
        {
            return this.name;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public NetworkHandler GetHandler()
        {
            return this.handler;
        }

        public void SetHandler(NetworkHandler handler)
        {
            this.handler = handler;
        }

        public Device GetDevice()
        {
            return this.device;
        }

        public void SetDevice(Device device)
        {
            this.device = device;
        }

        new
        public Boolean Equals(Object obj)
        {
            if(!(obj is Activator))
            {
                return false;
            }
            Activator activator = (Activator)obj;
            return (this == activator || (this.GetId().Equals(activator.GetId()) &&
                this.GetRank().Equals(activator.GetRank()) &&
                this.GetState().Equals(activator.GetState()) &&
                this.GetName().Equals(activator.GetName()) &&
                this.GetHandler().Equals(activator.GetHandler())));
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