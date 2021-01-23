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

namespace Recipes.Features.Categories
{
    public static class Create
    {

        public class Command : IRequest<Category>
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

        public class Handler : IRequestHandler<Command, Category>
        {
            private readonly RecipesDbContext context;

            public Handler(RecipesDbContext context)
            {
                this.context = context;
            }
            public async Task<Category> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Categories.Where(x => x.Name == request.Name).AnyAsync())
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Category = "Category already exists" });

                var category = new Category()
                {
                    Name = request.Name,
                    Id  = Guid.NewGuid().ToString()
                };

                context.Categories.Add(category);

                if(await context.SaveChangesAsync() > 0)
                {
                    return category;
                }

                throw new Exception("Problem creating category");
            }
        }

    }
}
