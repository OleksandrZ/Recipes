using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Features.DTOs;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using Recipes.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.UserProfile
{
    public static class FavoritedRecipes
    {
        public record RecipesEnvelope(List<ShortRecipeDto> Recipes, int RecipeCount);

        public class Query : IRequest<RecipesEnvelope>
        {
        }

        public class Handler : IRequestHandler<Query, RecipesEnvelope>
        {
            private readonly RecipesDbContext context;
            private readonly ICurrentUserAccessor currentUserAccessor;
            private readonly IMapper mapper;

            public Handler(ICurrentUserAccessor currentUserAccessor, RecipesDbContext context, IMapper mapper)
            {
                this.currentUserAccessor = currentUserAccessor;
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<RecipesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await context.Users.Include(x => x.RecipeFavorites)
                                              .ThenInclude(x => x.Recipe)
                                              .ThenInclude(x => x.MainImage)
                                              .Include(x => x.RecipeFavorites)
                                              .ThenInclude(x => x.Recipe)
                                              .ThenInclude(x => x.Author)
                                              .SingleOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

                var recipes = user.RecipeFavorites.OrderBy(x => x.Recipe.UpdatedAt).Select(o => o.Recipe).ToList();

                if (recipes == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, "Could not find favorite recipes");

                var recipesToReturn = mapper.Map<List<Recipe>, List<ShortRecipeDto>>(recipes);


                return new RecipesEnvelope(recipesToReturn, recipesToReturn.Count);
            }
        }
    }
}
