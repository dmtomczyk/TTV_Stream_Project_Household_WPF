using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace STR001.WPF.Models
{
    /// <summary>
    /// Used for serialization / deserialization of properties set in the .json file.
    /// </summary>
    public class UserPrefsJSON
    {

        [JsonProperty("Theme")]
        public string Theme { get; set; }

    }
}
