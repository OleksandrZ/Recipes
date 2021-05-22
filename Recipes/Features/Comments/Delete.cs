using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;

namespace Recipes.Features.Comments
{
    public static class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly RecipesDbContext context;

            public Handler(RecipesDbContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == request.Id,
                    cancellationToken: cancellationToken);

                if (comment == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {message = "Comment not found"});
                }

                context.Comments.Remove(comment);

                if (await context.SaveChangesAsync(cancellationToken) <= 0)
                {
                    throw new Exception("Problem saving changes");
                }

                return Unit.Value;
            }
        }
    }
}