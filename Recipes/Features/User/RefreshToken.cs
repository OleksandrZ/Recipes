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
using Recipes.Infrastructure.Interfaces;

namespace Recipes.Features.User
{
    public static class RefreshToken
    {
        public class Query : IRequest<User>
        {
            public string Token { get; set; }
            public string Ip { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly RecipesDbContext context;
            private readonly IJwtTokenGenerator jwtGenerator;
            private readonly IRefreshTokenGenerator refreshTokenGenerator;

            public Handler(RecipesDbContext context, IRefreshTokenGenerator refreshTokenGenerator,
                IJwtTokenGenerator jwtGenerator)
            {
                this.context = context;
                this.refreshTokenGenerator = refreshTokenGenerator;
                this.jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.Token))
                    throw new RestException(HttpStatusCode.BadRequest, new {Message = "Token must not be empty"});

                var user = await context.AppUsers
                    .Include(x => x.RefreshTokens)
                    .Include(x => x.Photo)
                    .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.Token),
                        cancellationToken: cancellationToken);

                if (user == null) throw new RestException(System.Net.HttpStatusCode.NotFound);

                var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

                if (!refreshToken.IsActive) throw new RestException(System.Net.HttpStatusCode.Forbidden);

                var newRefreshToken = refreshTokenGenerator.GenerateRefreshToken(request.Ip);
                refreshToken.Revoked = DateTime.Now;
                refreshToken.RevokedByIp = request.Ip;
                refreshToken.ReplacedByToken = newRefreshToken.Token;
                user.RefreshTokens.Add(newRefreshToken);

                context.Update(user);
                await context.SaveChangesAsync(cancellationToken);

                var jwtToken = await jwtGenerator.CreateToken(user);

                return new User()
                {
                    Username = user.UserName,
                    ImageUrl = user.Photo.Url,
                    JwtToken = jwtToken,
                    RefreshToken = newRefreshToken.Token
                };
            }
        }
    }
}