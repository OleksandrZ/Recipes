using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Features.Comments
{
    public class CommentDto
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserProfileImage { get; set; }
    }
}
