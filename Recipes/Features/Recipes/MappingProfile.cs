using AutoMapper;
using Recipes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recipes.Features.Comments;
using Recipes.Features.DTOs;

namespace Recipes.Features.Recipes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.UserProfileImageUrl, o => o.MapFrom(s => s.Author.Photo.Url));

            CreateMap<Recipe, RecipeDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Cuisine, o => o.MapFrom(s => s.Cuisine.Name))
                .ForMember(d => d.Difficulty, o => o.MapFrom(s => s.Difficulty.ToString()))
                .ForMember(d => d.Comments,
                    o => o.MapFrom(s => s.Comments));
            CreateMap<Recipe, ShortRecipeDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.MainImage.Url));
        }
    }
}