using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace github_release_poster
{
    /// <summary> Provides functionality for transforming objects to and from JSON </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Ingests a string in JSON format and returns a reference to an object
        /// whose properties are set to the values in the JSON.
        /// </summary>
        /// <param name="json">String containing the JSON to be parsed.</param>
        /// <returns>
        /// An instance of an object whose properties expose the JSON data
        /// provided.
        /// </returns>
        public static dynamic FromJson(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            return JObject.Parse(json);
        }

        /// <summary>
        /// Ingests a string in JSON format and returns a reference to an object
        /// whose properties are set to the values in the JSON.
        /// </summary>
        /// <param name="json">String containing the JSON to be parsed.</param>
        /// <returns>An instance of an object having the type T, if parseable.</returns>
        public static T FromJson<T>(this string json)
            => string.IsNullOrWhiteSpace(json)
                ? default
                : JObject.Parse(json)
                         .ToObject<T>();

        /// <summary> Exports the object referred to by <see cref="obj" /> to JSON. </summary>
        /// <param name="obj">Reference to an instance of the object you want to export.</param>
        /// <returns>
        /// String containing the JSON equivalent of the object, in a
        /// pretty-printed format.
        /// </returns>
        public static string ToJson<T>(this T obj)
            => JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}