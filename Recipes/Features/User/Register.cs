using FluentValidation;
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
    public static class Register
    {

        public class Command : IRequest<User>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Username).NotEmpty().MinimumLength(5).WithMessage("Username must be at least 5 characters");
                RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must have at least 1 lowercase character")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non alphanumeric");
            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly RecipesDbContext context;
            private readonly UserManager<AppUser> userManager;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager)
            {
                this.context = context;
                this.userManager = userManager;
            }
            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.AppUsers.Where(u => u.Email == request.Email).AnyAsync())
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Email = "Email already exsists" });

                if (await context.AppUsers.Where(u => u.UserName == request.Username).AnyAsync())
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Email = "Username already exsists" });

                var user = new AppUser()
                {
                    UserName = request.Username,
                    Email = request.Email,
                    Photo = new Photo() {Id = Guid.NewGuid().ToString()},
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, request.Password);


                if (result.Succeeded)
                {
                    return new User()
                    {
                        Username = user.UserName,
                        Image = user.Photo.Path ?? ""
                    };
                }

                throw new Exception("Problem creating user");
            }
        }

    }
}
