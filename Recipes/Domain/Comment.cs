using System;

namespace Recipes.Domain
{
    public class Comment
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public AppUser Author { get; set; }
        public Recipe Recipe { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}