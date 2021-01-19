using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly SignInManager<AppUser> SignInManager;
            private readonly UserManager<AppUser> userManager;
            private readonly IJwtTokenGenerator jwtGenerator;

            public Handler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IJwtTokenGenerator jwtGenerator)
            {
                SignInManager = signInManager;
                this.userManager = userManager;
                this.jwtGenerator = jwtGenerator;
            }
            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(System.Net.HttpStatusCode.Unauthorized);

                var result = await SignInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if(result.Succeeded)
                {
                    return new User()
                    {
                        Image = user.Photo == null ? "" : user.Photo.Path,
                        Token = jwtGenerator.CreateToken(user),
                        Username = user.UserName
                    };
                }
                throw new RestException(System.Net.HttpStatusCode.Unauthorized);
            }
        }

    }
}
