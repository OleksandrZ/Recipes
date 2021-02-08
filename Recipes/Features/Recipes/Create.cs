using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public static class Create
    {
        public class Command : IRequest
        {
            public string Cuisine { get; set; }
            public string Title { get; set; }
            public string Difficulty { get; set; }
            public ICollection<Ingredient> Ingredients { get; set; }
            public int Servings { get; set; }
            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan TimeOfCooking { get; set; }
            public NutritionValue NutritionValue { get; set; }
            public ICollection<Step> StepsOfCooking { get; set; }
            public ICollection<Category> Categories { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().MinimumLength(4);
                RuleFor(x => x.Servings).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Cuisine).NotEmpty();
                RuleFor(x => x.Difficulty).NotEmpty();
                RuleFor(x => x.NutritionValue).NotNull();
                RuleFor(x => x.StepsOfCooking).NotNull();
                RuleFor(x => x.Categories).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RecipesDbContext context;
            private readonly UserManager<AppUser> userManager;
            private ICurrentUserAccessor currentUserAccessor;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager, ICurrentUserAccessor currentUserAccessor)
            {
                this.context = context;
                this.userManager = userManager;
                this.currentUserAccessor = currentUserAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());

                var recipe = new Recipe()
                {
                    Author = user,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Servings = request.Servings,
                    Id = Guid.NewGuid().ToString(),
                    Title = request.Title,
                    TimeOfCooking = request.TimeOfCooking,
                    Ingredients = request.Ingredients,
                    Cuisine = context.Cuisines.Where(x => x.Name == request.Cuisine).FirstOrDefault(),
                    Difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), request.Difficulty),
                    NutritionValue = request.NutritionValue,
                    StepsOfCooking = request.StepsOfCooking
                };

                recipe.Categories = new List<Category>();
                foreach (var cat in request.Categories)
                {
                    recipe.Categories.Add(context.Categories.Where(x => x.Name == cat.Name).FirstOrDefault());
                }

                context.Recipes.Add(recipe);


                if(await context.SaveChangesAsync() > 0)
                    return Unit.Value;

                throw new Exception("Problem saving recipe");
            }
        }
    }
}