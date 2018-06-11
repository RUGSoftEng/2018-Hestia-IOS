using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Hestia.backend
{
    /// <summary>
    /// Static class with method that checks if a string is a json string.
    /// </summary>
    public static class JsonValidator
    {
        /// <summary>
        /// Checks if a string is a json string.
        /// </summary>
        /// <param name="json"></param>
        /// <returns>True or false</returns>
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
                    Console.WriteLine(ex);
                    return false;
                }
            }
            return false;
        }
    }
}
