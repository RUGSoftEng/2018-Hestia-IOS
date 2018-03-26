using System;
using System.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;


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
    public class ActivatorState<T>
    {
        private T rawState;
        private String type;

        public T RawState { get; set; }
        public String Type { get; set; }

        public ActivatorState(T rawState, String type)
        {
            this.rawState = rawState;
            this.type = type;
        }

        /*
         * Get the JSON representation of the RawS State, which could be a Boolean, a Float or a String.
         */
        public JsonPrimitive GetRawStateJSON ()
        {
            switch (this.GetType().ToString().ToLower())
            {
                case "bool":
                    return new JsonPrimitive(bool.Parse(this.RawState.ToString()));
                case "float":
                    return new JsonPrimitive(float.Parse(this.RawState.ToString()));
                default:
                    return new JsonPrimitive(this.RawState.ToString());
            }
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
            ActivatorState<T> activatorState = (ActivatorState<T>) obj;
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