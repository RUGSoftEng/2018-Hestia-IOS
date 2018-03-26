using Newtonsoft.Json.Linq;

namespace Hestia.backend.models.deserializers
{
    /**
     * Helper class that deserializes a json object into an Activator object
     */
    class ActivatorDeserializer
    {
        public Activator deserialize(JToken jT)
        {
            // get the activatorId, rank and name
            string id = jT.SelectToken("activatorId").ToString();
            int rank = jT.SelectToken("rank").ToObject<int>();
            string name = jT.SelectToken("name").ToString();

            // create the Activator
            Activator activator = new Activator(id, rank, name);

            // get the ActivatorState
            string type = jT.SelectToken("type").ToString();
            string rawState = jT.SelectToken("state").ToString();
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
