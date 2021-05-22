using AutoMapper;
using Recipes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Features.Comments;

namespace Recipes.Features.Recipes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.UserProfileImage, o => o.MapFrom(s => s.Author.Photo));

            CreateMap<Recipe, RecipeDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Cuisine, o => o.MapFrom(s => s.Cuisine.Name))
                .ForMember(d => d.Difficulty, o => o.MapFrom(s => s.Difficulty.ToString()))
                .ForMember(d => d.Comments,
                    o => o.MapFrom(s => s.Comments));
        }
    }
}