using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Cuisines
{
    public static class List
    {
        public class CuisinesEnvelope
        {
            public ICollection<Cuisine> Cuisines { get; set; }
            public int CuisineCount { get; set; }
        }

        public class Query : IRequest<CuisinesEnvelope> { }

        public class Handler : IRequestHandler<Query, CuisinesEnvelope>
        {
            private readonly RecipesDbContext context;
            public Handler(RecipesDbContext context)
            {
                this.context = context;
            }
            public async Task<CuisinesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var cuisines = await context.Cuisines.ToListAsync();

                return new CuisinesEnvelope()
                {
                    Cuisines= cuisines,
                    CuisineCount = cuisines.Count
                };
            }
        }
    }
}
