﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Recipes.Domain
{
    public class Recipe
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Cuisine Cuisine { get; set; }
        public AppUser Author { get; set; }
        public string Title { get; set; }
        public Difficulty Difficulty { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public int Servings { get; set; }
        public TimeSpan TimeOfCooking { get; set; }
        public Photo MainImage { get; set; }
        public NutritionValue NutritionValue { get; set; }
        public int SiteVisits { get; set; }
        public ICollection<Step> StepsOfCooking { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Category> Categories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<RecipeFavorite> RecipeFavorites { get; set; }
        public ICollection<RecipeLike> RecipeLikes { get; set; }
    }
}