using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using Recipes.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public static class RemoveFromFavorite
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RecipesDbContext context;
            private readonly ICurrentUserAccessor currentUserAccessor;

            public Handler(ICurrentUserAccessor currentUserAccessor, RecipesDbContext context)
            {
                this.currentUserAccessor = currentUserAccessor;
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var recipe = await context.Recipes.FindAsync(request.Id, cancellationToken);

                if (recipe is null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = "Could not find recipe" });

                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

                var favorite = await context.RecipeFavorites.SingleOrDefaultAsync(x => x.RecipeId == recipe.Id && x.UserId == user.Id, cancellationToken: cancellationToken);

                if (favorite == null)
                    return Unit.Value;

                context.RecipeFavorites.Remove(favorite);

                if (await context.SaveChangesAsync(cancellationToken) > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}
