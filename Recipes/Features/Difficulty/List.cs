using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;

namespace Recipes.Features.Difficulty
{
    public static class List
    {
        public class Query : IRequest<List<string>> { }

        public class Handler : IRequestHandler<Query, List<string>>
        {
            private readonly RecipesDbContext context;
            public Handler(RecipesDbContext context)
            {
                this.context = context;
            }
            public async Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var difficulties = new List<string>
                {
                    Domain.Difficulty.Begginer.ToString(),
                    Domain.Difficulty.Easy.ToString(),
                    Domain.Difficulty.Normal.ToString(),
                    Domain.Difficulty.Hard.ToString(),
                    Domain.Difficulty.Expert.ToString()
                };

                return difficulties;
            }
        }
    }
}
