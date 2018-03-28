using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hestia.backend.models
{
    public class RequiredInfo
    {
        private string collection;
        private string plugin;
        private Dictionary<string, string> info;

        [JsonProperty("collection")]
        public string Collection { get; set; }
        [JsonProperty("plugin_name")]
        public string Plugin { get; set; }
        [JsonProperty("required_info")]
        public Dictionary<string, string> Info { get; set; }

        public RequiredInfo(string collection, string plugin, Dictionary<String, String> info)
        {
            this.collection = collection;
            this.plugin = plugin;
            this.info = info;
        }
    }
}
