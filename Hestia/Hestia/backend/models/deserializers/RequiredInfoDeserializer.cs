using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.backend.models.deserializers

{
    // Class that deserializes a JToken into a RequiredInfo object
    class RequiredInfoDeserializer
    {
        public RequiredInfo deserialize(JToken jT)
        {
            // get collection, plugin and info
            string collection = jT.Value<string>("collection");
            string plugin = jT.Value<string>("plugin_name");
            Dictionary<string, string> info = jT.Value<Dictionary<string, string>>("required_info");

            // return a RequiredInfo object
            return new RequiredInfo(collection, plugin, info);
        }
    }
}
