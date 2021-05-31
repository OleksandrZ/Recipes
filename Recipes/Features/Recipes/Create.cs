using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Recipes.Domain;
using Recipes.Features.DTOs;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
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
            public string Description { get; set; }
            public string Difficulty { get; set; }
            public ICollection<Ingredient> Ingredients { get; set; }
            public int Servings { get; set; }
            [JsonConverter(typeof(TimeSpanConverter))]
            public TimeSpan TimeOfCooking { get; set; }
            public NutritionValue NutritionValue { get; set; }
            public ICollection<StepDto> StepsOfCooking { get; set; }
            public ICollection<Category> Categories { get; set; }
            public IFormFile MainImage { get; set; }
            public ICollection<IFormFile> Images { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().MinimumLength(4);
                RuleFor(x => x.Servings).NotEmpty().GreaterThan(0);
                RuleFor(x => x.Cuisine).NotEmpty();
                RuleFor(x => x.Description).MinimumLength(10);
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
            private readonly ICurrentUserAccessor currentUserAccessor;
            private readonly IPhotoAccessor photoAccessor;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager, ICurrentUserAccessor currentUserAccessor, IPhotoAccessor photoAccessor)
            {
                this.context = context;
                this.userManager = userManager;
                this.currentUserAccessor = currentUserAccessor;
                this.photoAccessor = photoAccessor;
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
                    Description = request.Description,
                    Id = Guid.NewGuid().ToString(),
                    Title = request.Title,
                    TimeOfCooking = request.TimeOfCooking,
                    Ingredients = request.Ingredients,
                    Cuisine = context.Cuisines.FirstOrDefault(x => x.Name == request.Cuisine),
                    NutritionValue = request.NutritionValue
                };

                if(Enum.TryParse(typeof(Domain.Difficulty), request.Difficulty,  out object res))
                {
                    if (res != null) recipe.Difficulty = (Domain.Difficulty) res;
                }
                else
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = "Wrong difficulty" });
                }

                recipe.Categories = new List<Category>();
                foreach (var cat in request.Categories)
                {
                    recipe.Categories.Add(context.Categories.FirstOrDefault(x => x.Name == cat.Name));
                }

                if (request.MainImage == null)
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { message = "Main image is required" });

                recipe.MainImage = photoAccessor.CreatePhoto(request.MainImage);

                if (request.Images is {Count: > 0})
                {
                    recipe.StepsOfCooking = new List<Step>();
                    foreach (var stepDto in request.StepsOfCooking)
                    {
                        recipe.StepsOfCooking.Add(new Step()
                        {
                            Description = stepDto.Description,
                            Image = photoAccessor.CreatePhoto(request.Images.SingleOrDefault(x => x.FileName == stepDto.ImageName)),
                            StepNumber = stepDto.StepNumber
                        });
                    }
                }


                context.Recipes.Add(recipe);


                if(await context.SaveChangesAsync(cancellationToken) > 0)
                    return Unit.Value;

                throw new Exception("Problem saving recipe");
            }
        }
    }
}