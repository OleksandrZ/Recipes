using System;
using System.Text.Json.Serialization;

namespace Recipes.Domain
{
    public class Photo
    {
        public string Id { get; set; }
        [JsonIgnore]
        public string Path { get; set; }
        [JsonIgnore]
        public string FileName { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public long Size { get; set; }
    }
}