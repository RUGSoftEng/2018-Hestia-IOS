using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hestia.backend.models
{
    /// <summary>
    /// This class holds the data that needs to be sent to the server in order to add a new device.
    /// It contains the strings collection and plugin. The dictionary requiredInfo holds key-value pairs with required information.
    /// </summary>
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
