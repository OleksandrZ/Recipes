using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using Recipes.Domain;
using Recipes.Features.DTOs;
using Recipes.Infrastructure;

namespace Recipes.Features.Comments
{
    public static class List
    {
        public class Query : IRequest<List<CommentDto>>
        {
            public string RecipeId { get; set; }
        }
        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RecipeId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Query, List<CommentDto>>
        {
            private readonly RecipesDbContext context;
            private readonly IMapper mapper;

            public Handler(RecipesDbContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task<List<CommentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await context.Comments
                    .Include(x => x.Author)
                    .Include(x => x.Recipe)
                    .Where(x => x.Recipe.Id == request.RecipeId)
                    .ToListAsync(cancellationToken: cancellationToken);

                return mapper.Map<List<Comment>, List<CommentDto>>(comments);
            }
        }
    }
}