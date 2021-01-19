using System;

namespace Recipes.Domain
{
    public class NutrionValue
    {
        public string Id { get; set; }
        public int? Value { get; set; }
        public int? Proteins { get; set; }
        public int? Fats { get; set; }
        public int? Сarbohydrates { get; set; }

    }
}