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

namespace Recipes.Features.DTOs
{
    public static class AddToFavorite
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

            public Handler(RecipesDbContext context, ICurrentUserAccessor currentUserAccessor)
            {
                this.context = context;
                this.currentUserAccessor = currentUserAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var recipe = await context.Recipes.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

                if (recipe == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { message = "Could not find recipe" });

                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

                var favorited = await context.RecipeFavorites.SingleOrDefaultAsync(x => x.RecipeId == recipe.Id && x.UserId == user.Id, cancellationToken: cancellationToken);

                if (favorited != null)
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = "Already added to favorite" });

                favorited = new Domain.RecipeFavorite
                {
                    Recipe = recipe,
                    User = user
                };

                context.RecipeFavorites.Add(favorited);

                if((await context.SaveChangesAsync(cancellationToken) > 0))
                {
                    return Unit.Value;
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}
