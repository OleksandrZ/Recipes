using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Features.DTOs;
using Recipes.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public static class List
    {
        public record RecipesEnvelope(List<RecipeDto> Recipes, int RecipeCount);
        public class Query : IRequest<RecipesEnvelope>
        {
            public List<Category> Categories { get; set; }
            public string[] Difficulties { get; set; }
        }

        public class Handler : IRequestHandler<Query, RecipesEnvelope>
        {
            private readonly RecipesDbContext context;
            private readonly UserManager<AppUser> userManager;
            private readonly IMapper mapper;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager, IMapper mapper)
            {
                this.context = context;
                this.userManager = userManager;
                this.mapper = mapper;
            }
            public async Task<RecipesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var recipes = await context.Recipes
                    .Include(x => x.Author)
                    .Include(x => x.Cuisine)
                    .Include(x => x.Categories)
                    .Include(x => x.Comments)
                    .Include(x => x.NutritionValue)
                    .Include(x => x.StepsOfCooking)
                    .ThenInclude(x => x.Image)
                    .Include(x => x.Ingredients)
                    .Include(x => x.MainImage)
                    .OrderBy(x => x.UpdatedAt).ToListAsync(cancellationToken: cancellationToken);
                List<Recipe> sortedByCategoryRecipes = new();
                foreach (var category in request.Categories)
                {
                    sortedByCategoryRecipes.AddRange(recipes.FindAll(x => x.Categories.Contains(category)));
                }
                if (request.Difficulties.Length == 0)
                {
                    return new RecipesEnvelope(mapper.Map<List<Recipe>, List<RecipeDto>>(sortedByCategoryRecipes), sortedByCategoryRecipes.Count);
                }
                List<Recipe> sortedByDifficultyRecipes = new List<Recipe>();
                foreach (var difficulty in request.Difficulties)
                {
                    sortedByDifficultyRecipes.AddRange(recipes.FindAll(x => x.Difficulty.ToString() == difficulty));
                }

                if (sortedByCategoryRecipes.Count > 0 && sortedByDifficultyRecipes.Count > 0)
                {
                    var intersectRecipes = sortedByCategoryRecipes.Intersect(sortedByDifficultyRecipes).ToList();
                    return new RecipesEnvelope(mapper.Map<List<Recipe>, List<RecipeDto>>(intersectRecipes), intersectRecipes.Count);
                }

                return new RecipesEnvelope(mapper.Map<List<Recipe>, List<RecipeDto>>(recipes), recipes.Count);
            }
        }

    }
}
