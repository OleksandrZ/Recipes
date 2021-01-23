using System;
using System.Text.Json.Serialization;

namespace Recipes.Domain
{
    public class Category
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}