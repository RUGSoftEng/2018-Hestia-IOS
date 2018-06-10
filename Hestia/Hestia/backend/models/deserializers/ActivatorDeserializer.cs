using Newtonsoft.Json.Linq;

namespace Hestia.Backend.Models.Deserializers
{
    /**
     * Helper class that deserializes a json object into an Activator object
     */
    public class ActivatorDeserializer
    {
        public Activator DeserializeActivator(JToken jsonActivator)
        {
            // get the activatorId, rank and name
            string id = jsonActivator.Value<string>("activatorId");
            int rank = jsonActivator.Value<int>("rank");
            string name = jsonActivator.Value<string>("name");
            
            // get the state type
            string type = jsonActivator.Value<string>("type");
            ActivatorState state = null;

            switch (type.ToLower())
            {
                case "bool":
                    state = new ActivatorState(jsonActivator.Value<bool>("state"), "bool");
                    break;
                case "float":
                    state = new ActivatorState(jsonActivator.Value<float>("state"), "float");
                    break;
            }

            // create the Activator
            Activator activator = new Activator(id, name, rank, state);

            return activator;
        }
    }
}
