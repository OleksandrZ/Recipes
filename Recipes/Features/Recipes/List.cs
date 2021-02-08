using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public static class List
    {
        public record RecipesEnvelope(List<RecipeDto> Recipes, int RecipeCount);
        public class Query : IRequest<RecipesEnvelope> { }

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
                    .Include(x => x.Ingredients)
                    .OrderBy(x => x.UpdatedAt).ToListAsync();

                return new RecipesEnvelope(mapper.Map<List<Recipe>, List<RecipeDto>>(recipes), recipes.Count);
            }
        }

    }
}
