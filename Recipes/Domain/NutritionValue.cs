using System;

namespace Recipes.Domain
{
    public class NutritionValue
    {
        public string Id { get; set; }
        public int? Value { get; set; }
        public int? Proteins { get; set; }
        public int? Fats { get; set; }
        public int? Сarbohydrates { get; set; }

    }
}