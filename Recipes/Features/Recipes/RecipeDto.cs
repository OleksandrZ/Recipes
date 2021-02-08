using Recipes.Domain;
using Recipes.Features.Comments;
using System;
using System.Collections.Generic;

namespace Recipes.Features.Recipes
{
    public class RecipeDto
    {
        public string Id { get; set; }
        public string Cuisine { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Difficulty { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public int Servings { get; set; }
        public TimeSpan TimeOfCooking { get; set; }
        public ICollection<Photo> Images { get; set; }
        public NutritionValue NutritionValue { get; set; }
        public ICollection<Step> StepsOfCooking { get; set; }
        public ICollection<Category> Categories { get; set; }
        public DateTime Date { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }


}