using FluentValidation;
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

namespace Recipes.Features.Cuisines
{
    public static class Create
    {
        public class Command : IRequest<Cuisine>
        {
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).MinimumLength(3);
            }
        }

        public class Handler : IRequestHandler<Command, Cuisine>
        {
            private readonly RecipesDbContext context;

            public Handler(RecipesDbContext context)
            {
                this.context = context;
            }
            public async Task<Cuisine> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Cuisine.Where(x => x.Name == request.Name).AnyAsync())
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Cuisine = "Cuisine already exists" });

                var cuisine = new Cuisine()
                {
                    Name = request.Name,
                    Id = Guid.NewGuid().ToString()
                };

                context.Cuisines.Add(cuisine);

                if (await context.SaveChangesAsync() > 0)
                {
                    return cuisine;
                }

                throw new Exception("Problem creating cuisine");
            }
        }
    }
}
