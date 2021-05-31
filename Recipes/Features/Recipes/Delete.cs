using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
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
            private readonly IPhotoAccessor photoAccessor;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager, IPhotoAccessor photoAccessor)
            {
                this.context = context;
                this.userManager = userManager;
                this.photoAccessor = photoAccessor;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var recipe = await context
                    .Recipes
                    .Include(x => x.Ingredients)
                    .Include(x => x.StepsOfCooking)
                    .ThenInclude(x => x.Image)
                    .Include(x => x.Categories)
                    .Include(x => x.Comments)
                    .Include(x => x.Cuisine)
                    .Include(x => x.MainImage)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (recipe == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { Recipe = "Recipe not found" });

                foreach (var step in recipe.StepsOfCooking)
                {
                    await photoAccessor.DeletePhotoAsync(step.Image);
                }

                await photoAccessor.DeletePhotoAsync(recipe.MainImage);

                context.Remove(recipe);



                if (await context.SaveChangesAsync(cancellationToken) > 0)
                    return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
