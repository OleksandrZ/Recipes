using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipes.Domain
{
    public class Ingredient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Amount { get; set; }
    }
}