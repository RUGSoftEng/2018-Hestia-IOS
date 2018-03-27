using Newtonsoft.Json.Linq;

namespace Hestia.backend.models.deserializers
{
    /**
     * Helper class that deserializes a json object into an Activator object
     */
    class ActivatorDeserializer
    {
        public Activator Deserialize(JToken jT)
        {
            // get the activatorId, rank and name
            string id = jT.Value<string>("activatorId");
            int rank = jT.Value<int>("rank");
            string name = jT.Value<string>("name");

            // create the Activator
            Activator activator = new Activator(id, rank, name);

            // get the ActivatorState
            string type = jT.Value<string>("type");
            string rawState = jT.Value<string>("state");
            ActivatorState<object> state = null;

            switch (type.ToLower())
            {
                case "bool":
                    state = new ActivatorState<object>(bool.Parse(rawState), "bool");
                    break;
                case "float":
                    state = new ActivatorState<object>(float.Parse(rawState), "float");
                    break;
                default:
                    break;
            }

            activator.State = state;

            return activator;
        }

    }
}
