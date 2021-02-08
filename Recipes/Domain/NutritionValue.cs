using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Domain
{
    public class NutritionValue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int? Value { get; set; }
        public int? Proteins { get; set; }
        public int? Fats { get; set; }
        public int? Сarbohydrates { get; set; }

    }
}