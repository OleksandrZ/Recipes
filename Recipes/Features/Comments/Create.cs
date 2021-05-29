using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using Recipes.Infrastructure.Interfaces;

namespace Recipes.Features.Comments
{
    public static class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public string RecipeId { get; set; }
            public string Body { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
                RuleFor(x => x.RecipeId).MinimumLength(10);
            }
        }

        public class Handler : IRequestHandler<Command, CommentDto>
        {
            private readonly RecipesDbContext context;
            private readonly UserManager<AppUser> userManager;
            private readonly ICurrentUserAccessor currentUserAccessor;
            private readonly IMapper mapper;

            public Handler(RecipesDbContext context, ICurrentUserAccessor currentUserAccessor, UserManager<AppUser> userManager, IMapper mapper)
            {
                this.context = context;
                this.currentUserAccessor = currentUserAccessor;
                this.userManager = userManager;
                this.mapper = mapper;
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());

                var recipe = await context.Recipes.FirstOrDefaultAsync(x => x.Id == request.RecipeId, cancellationToken: cancellationToken);

                if (recipe == null)
                    throw new RestException(HttpStatusCode.NotFound, new {message = "Recipe not found"});

                var comment = new Comment()
                {
                    Body = request.Body,
                    Recipe = recipe,
                    Author = user,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                };

                await context.Comments.AddAsync(comment, cancellationToken);
                if (await context.SaveChangesAsync(cancellationToken) > 0)
                {
                    return mapper.Map<Comment, CommentDto>(comment);
                }

                throw new Exception("Problem when creating comment");

            }
        }
    }
}
