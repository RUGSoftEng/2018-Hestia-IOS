using System;

namespace Hestia.Backend.Models
{
    /// <summary>
    /// Wrapper class for the different state fields.
    /// </summary>
    public class ActivatorState
    {
        object rawState;
        string type;

        public object RawState
        {
            get => rawState;
            set => rawState = value;
        }
        public string Type
        {
            get => type;
            set => type = value;
        }

        public ActivatorState(object rawState, string type)
        {
            this.rawState = rawState;
            this.type = type;
        }
        
        public override string ToString()
        {
            return type + " " + rawState;
        }
    }
}
