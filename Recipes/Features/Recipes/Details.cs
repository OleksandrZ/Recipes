using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public static class Details
    {
        public class Query : IRequest<RecipeDto>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, RecipeDto>
        {
            private readonly RecipesDbContext context;
            private readonly IMapper mapper;

            public Handler(IMapper mapper, RecipesDbContext context)
            {
                this.mapper = mapper;
                this.context = context;
            }
            public async Task<RecipeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var recipes = await context.Recipes
                   .Include(x => x.Author)
                   .Include(x => x.Cuisine)
                   .Include(x => x.Categories)
                   .Include(x => x.Comments)
                   .Include(x => x.NutritionValue)
                   .Include(x => x.StepsOfCooking)
                   .Include(x => x.Ingredients)
                   .OrderBy(x => x.UpdatedAt).ToListAsync();

                var recipe = recipes.Find(x => x.Id == request.Id);

                if (recipe == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { Recipe = "Not found" });

                recipe.SiteVisits++;
                return mapper.Map<Recipe, RecipeDto>(recipe);
            }
        }

    }
}
