using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Domain
{
    public class RecipeFavorite
    {
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
