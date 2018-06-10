using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hestia.Backend.Models
{
    public class PluginInfo
    {
        string collection;
        string plugin;
        Dictionary<string, string> requiredInfo;

        [JsonProperty("collection")]
        public string Collection
        {
            get => collection;
            set => collection = value;
        }
        [JsonProperty("plugin_name")]
        public string Plugin
        {
            get => plugin;
            set => plugin = value;
        }
        [JsonProperty("required_info")]
        public Dictionary<string, string> RequiredInfo
        {
            get => requiredInfo;
            set => requiredInfo = value;
        }

        public PluginInfo(string collection, string plugin, Dictionary<String, String> requiredInfo)
        {
            this.collection = collection;
            this.plugin = plugin;
            this.requiredInfo = requiredInfo;
        }
    }
}
