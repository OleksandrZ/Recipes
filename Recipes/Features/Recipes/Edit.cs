using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Recipes
{
    public static class Edit
    {

        public class Command : IRequest
        {
            public string Id { get; set; }
            public string Cuisine { get; set; }
            public string Title { get; set; }
            public string Difficulty { get; set; }
            public ICollection<Ingredient> Ingredients { get; set; }
            public int? Servings { get; set; }
            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan? TimeOfCooking { get; set; }
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

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager)
            {
                this.context = context;
                this.userManager = userManager;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var recipes = await context.Recipes
                    .Include(x => x.Categories)
                    .Include(x => x.Cuisine)
                    .Include(x => x.Ingredients)
                    .Include(x => x.NutritionValue)
                    .Include(x => x.StepsOfCooking)
                    .ToListAsync();

                var recipe = recipes.Find(x => x.Id == request.Id);

                if (recipe == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new { Recipe = "Recipe not found" });

                Difficulty diff = recipe.Difficulty;
                object obj;
                if (Enum.TryParse(typeof(Difficulty), request.Difficulty, true, out obj) && obj is Difficulty difficulty)
                {
                    diff = difficulty;
                }

                recipe.Servings = request.Servings ?? recipe.Servings;
                recipe.Title = request.Title ?? recipe.Title;
                recipe.TimeOfCooking = request.TimeOfCooking ?? recipe.TimeOfCooking;
                recipe.Ingredients = request.Ingredients ?? recipe.Ingredients;
                recipe.Cuisine = context.Cuisines.Where(x => x.Name == request.Cuisine).FirstOrDefault() ?? recipe.Cuisine;
                recipe.Difficulty = diff;
                recipe.NutritionValue = request.NutritionValue ?? recipe.NutritionValue;
                recipe.StepsOfCooking = request.StepsOfCooking ?? recipe.StepsOfCooking;

                var categories = new List<Category>();
                foreach (var cat in request.Categories)
                {
                    categories.Add(context.Categories.Where(x => x.Name == cat.Name).FirstOrDefault());
                }

                recipe.Categories = categories ?? recipe.Categories;
                

                if (context.ChangeTracker.Entries().First(x => x.Entity == recipe).State == EntityState.Modified)
                {
                    recipe.UpdatedAt = DateTime.Now;
                }

                if(await context.SaveChangesAsync() > 0)
                    return Unit.Value;

                throw new Exception("Problem saving recipe");

            }
        }

    }
}
