using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;

namespace Recipes.Features.User
{
    public static class Logout
    {
        public class Command : IRequest
        {
            public string Token { get; set; }
            public string Ip { get; set; }
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
                if (string.IsNullOrEmpty(request.Token))
                    throw new RestException(HttpStatusCode.BadRequest, new { Message = "Token must not be empty" });

                var user = await context.AppUsers
                    .Include(x => x.RefreshTokens)
                    .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token),
                        cancellationToken: cancellationToken);

                if (user == null) throw new RestException(System.Net.HttpStatusCode.NotFound);

                var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

                if (!refreshToken.IsActive) throw new RestException(System.Net.HttpStatusCode.Unauthorized);

                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.RevokedByIp = request.Ip;

                context.Update(user);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}