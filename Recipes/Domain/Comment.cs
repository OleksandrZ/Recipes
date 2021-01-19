using System;

namespace Recipes.Domain
{
    public class Comment
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public AppUser Author { get; set; }
        public Recipe Recipe { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}