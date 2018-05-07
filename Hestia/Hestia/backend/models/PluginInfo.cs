using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hestia.backend.models
{
    public class PluginInfo
    {
        private string collection;
        private string plugin;
        private Dictionary<string, string> requiredInfo;

        [JsonProperty("collection")]
        public string Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }
        [JsonProperty("plugin_name")]
        public string Plugin
        {
            get
            {
                return plugin;
            }
            set
            {
                plugin = value;
            }
        }
        [JsonProperty("required_info")]
        public Dictionary<string, string> RequiredInfo
        {
            get
            {
                return requiredInfo;
            }
            set
            {
                requiredInfo = value;
            }
        }

        public PluginInfo(string collection, string plugin, Dictionary<String, String> requiredInfo)
        {
            this.collection = collection;
            this.plugin = plugin;
            this.requiredInfo = requiredInfo;
        }
    }
}
