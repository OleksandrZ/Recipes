using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public static class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RecipesDbContext context;
            private readonly UserManager<AppUser> userManager;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager)
            {
                this.context = context;
                this.userManager = userManager;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var recipe = await context.Recipes.FindAsync(request.Id);

                if (recipe == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { Recipe = "Recipe not found" });

                context.Remove(recipe);

                if (await context.SaveChangesAsync() > 0)
                    return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
