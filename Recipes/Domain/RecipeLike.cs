using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Domain
{
    public class RecipeLike
    {
        public string LikedByUserId { get; set; }
        public AppUser LikedByUser { get; set; }
        public string LikedRecipeId { get; set; }
        public Recipe LikedRecipe { get; set; }
    }
}
