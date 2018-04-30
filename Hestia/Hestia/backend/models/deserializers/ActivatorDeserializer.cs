using Newtonsoft.Json.Linq;

namespace Hestia.backend.models.deserializers
{
    /**
     * Helper class that deserializes a json object into an Activator object
     */
    public class ActivatorDeserializer
    {
        public Activator Deserialize(JToken jsonActivator)
        {
            // get the activatorId, rank and name
            string id = jsonActivator.Value<string>("activatorId");
            int rank = jsonActivator.Value<int>("rank");
            string name = jsonActivator.Value<string>("name");

            // get the ActivatorState
            string type = jsonActivator.Value<string>("type");
            string rawState = jsonActivator.Value<string>("state");
            ActivatorState state = null;

            switch (type.ToLower())
            {
                case "bool":
                    state = new ActivatorState(bool.Parse(rawState), "bool");
                    break;
                case "float":
                    state = new ActivatorState(float.Parse(rawState), "float");
                    break;
                default:
                    break;
            }

            // create the Activator
            Activator activator = new Activator(id, name, rank, state);

            return activator;
        }
    }
}
