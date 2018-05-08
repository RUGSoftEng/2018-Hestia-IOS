using System;

/*
 * Wrapper class for the different state fields. The activator state has a type T, which is
 * inferred using a custom JSON deserializer.
 *
 * param <T> Type of the state of the activator. This can be a boolean (for a switch) or a float
 *            (for a slider).
 * see ActivatorDeserializer
 */
namespace Hestia.backend.models
{
    public class ActivatorState
    {
        private object rawState;
        private string type;

        public object RawState
        {
            get
            {
                return rawState;
            }
            set
            {
                rawState = value;
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public ActivatorState(object rawState, string type)
        {
            this.rawState = rawState;
            this.type = type;
        }
        
        override
        public String ToString()
        {
            return this.type + " - " + this.rawState.ToString();
        }

        new
        public bool Equals(Object obj)
        {
            if (!(obj.GetType() == this.GetType())) return false;
            ActivatorState activatorState = (ActivatorState) obj;
            return (this == activatorState || (this.GetType().Equals(activatorState.GetType()) &&
                    this.RawState.Equals(activatorState.RawState)));
        }

        new
        public int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}