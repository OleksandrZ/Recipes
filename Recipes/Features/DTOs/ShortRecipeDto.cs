using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Features.DTOs
{
    public class ShortRecipeDto
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
        public string Image { get; set; }
    }
}
