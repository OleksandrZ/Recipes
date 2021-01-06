using System;

namespace Recipes.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public User Author { get; set; }
        public Recipe Recipe { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}