﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Recipes.Domain
{
    public class AppUser : IdentityUser
    {
        [JsonIgnore]
        public ICollection<Recipe> Recipes { get; set; }
        [JsonIgnore]
        public ICollection<RecipeFavorite> RecipeFavorites { get; set; }
        [JsonIgnore]
        public ICollection<FollowedUsers> Following { get; set; }
        [JsonIgnore]
        public ICollection<FollowedUsers> Followers { get; set; }
        [JsonIgnore]
        public ICollection<Comment> Comments { get; set; }
        public Photo Photo { get; set; }
        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; }
        public ICollection<RecipeLike> LikedRecipes { get; set; }

    }
}
