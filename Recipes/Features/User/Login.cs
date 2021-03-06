﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using Recipes.Infrastructure.Interfaces;
using Recipes.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.User
{
    public static class Login
    {

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Query : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
            public string Ip { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly SignInManager<AppUser> SignInManager;
            private readonly UserManager<AppUser> userManager;
            private readonly IJwtTokenGenerator jwtGenerator;
            private readonly IRefreshTokenGenerator refreshTokenGenerator;
            private readonly RecipesDbContext context;

            public Handler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IJwtTokenGenerator jwtGenerator, IRefreshTokenGenerator refreshTokenGenerator, RecipesDbContext context)
            {
                SignInManager = signInManager;
                this.userManager = userManager;
                this.jwtGenerator = jwtGenerator;
                this.refreshTokenGenerator = refreshTokenGenerator;
                this.context = context;
            }
            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(System.Net.HttpStatusCode.Unauthorized);

                await context.Images.LoadAsync(cancellationToken: cancellationToken);
                var result = await SignInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);

                if(result.Succeeded)
                {
                    var refreshToken = refreshTokenGenerator.GenerateRefreshToken(request.Ip);
                    user.RefreshTokens.Add(refreshToken);
                    await userManager.UpdateAsync(user);

                    return new User()
                    {
                        ImageUrl = user.Photo == null ? "" : user.Photo.Url,
                        JwtToken = await jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        RefreshToken = refreshToken.Token
                    };
                }
                throw new RestException(System.Net.HttpStatusCode.Unauthorized);
            }
        }

    }
}
