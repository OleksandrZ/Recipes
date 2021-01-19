using System;

namespace Recipes.Domain
{
    public class Ingredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Amount { get; set; }
    }
}