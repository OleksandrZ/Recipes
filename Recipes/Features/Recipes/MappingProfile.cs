using AutoMapper;
using Recipes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeDto>()
                .ForMember(d => d.Author, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Cuisine, o => o.MapFrom(s => s.Cuisine.Name))
                .ForMember(d => d.Difficulty, o => o.MapFrom(s => s.Difficulty.ToString()));

        }
    }
}
