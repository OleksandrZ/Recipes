using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Categories
{
    public static class List
    {
        public record CategoriesEnvelope(ICollection<Category> Categories, int CategoryCount);

        public class Query : IRequest<CategoriesEnvelope> { }

        public class Handler : IRequestHandler<Query, CategoriesEnvelope>
        {
            private readonly RecipesDbContext context;
            public Handler(RecipesDbContext context)
            {
                this.context = context;
            }
            public async Task<CategoriesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var categories = await context.Categories.ToListAsync(cancellationToken: cancellationToken);

                return new CategoriesEnvelope(categories, categories.Count);
            }
        }

    }
}
