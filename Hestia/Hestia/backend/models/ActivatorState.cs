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
    class ActivatorState<T>
    {
        private T rawState;
        private String type;

        public ActivatorState(T rawState, String type)
        {
            this.rawState = rawState;
            this.type = type;
        }

        public T GetRawState()
        {
            return this.rawState;
        }

        public void SetRawState(T rawState)
        {
            this.rawState = rawState;
        }

        public void SetType(String type)
        {
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
                    return new JsonPrimitive(bool.Parse(this.GetRawState().ToString()));
                case "float":
                    return new JsonPrimitive(float.Parse(this.GetRawState().ToString()));
                default:
                    return new JsonPrimitive(this.GetRawState().ToString());
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
                    this.GetRawState().Equals(activatorState.GetRawState())));
        }

        new
        public int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}