using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hestia.backend.models
{
    class RequiredInfo
    {
        private string collection;
        private string plugin;
        private Dictionary<string, string> info;

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
        public Dictionary<string, string> Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
            }
        }

        public RequiredInfo(string collection, string plugin, Dictionary<String, String> info)
        {
            this.collection = collection;
            this.plugin = plugin;
            this.info = info;
        }
    }
}
