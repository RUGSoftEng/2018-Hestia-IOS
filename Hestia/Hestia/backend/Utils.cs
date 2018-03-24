using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIKit;

namespace Hestia.backend
{
    public static class Utils
    {
        public static bool IsValidJson(string json)
        {
            json = json.Trim();
            if ((json.StartsWith("{") && json.EndsWith("}")) || // JSON object
                (json.StartsWith("[") && json.EndsWith("]")))   // JSON array
            {
                try
                {
                    JToken.Parse(json);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
