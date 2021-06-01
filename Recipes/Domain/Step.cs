using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Recipes.Domain
{
    public class Step
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public string Id { get; set; }
        public string Description { get; set; }
        public int StepNumber { get; set; }
        public Photo Image { get; set; }
    }
}