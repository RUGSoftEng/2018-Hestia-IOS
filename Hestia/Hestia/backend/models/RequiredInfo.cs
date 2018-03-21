using System;
using System.Collections.Generic;
using System.Text;

namespace Hestia.backend.models
{
    class RequiredInfo
    {
        private String collection;
        private String plugin;
        private Dictionary<String, String> info;

        public RequiredInfo(String collection, String plugin, Dictionary<String, String> info)
        {
            this.Collection = collection;
            this.Plugin = plugin;
            this.Info = info;
        }

        public string Collection { get; set; }
        public string Plugin { get; set; }
        public Dictionary<string, string> Info { get; set; }
    }
}
