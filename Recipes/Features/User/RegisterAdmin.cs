using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.User
{
    public static class RegisterAdmin
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
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
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
            private readonly RoleManager<IdentityRole> roleManager;

            public Handler(RecipesDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                this.context = context;
                this.userManager = userManager;
                this.roleManager = roleManager;
            }
            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if(await context.AppUsers.Where(x => x.Email == request.Email).AnyAsync(cancellationToken: cancellationToken))
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Email = "Email already exsists" });

                if (await context.AppUsers.Where(u => u.UserName == request.Username).AnyAsync(cancellationToken: cancellationToken))
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new { Email = "Username already exsists" });

                AppUser user = new AppUser()
                {
                    Email = request.Email,
                    UserName = request.Username,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Photo = new Photo() { Id = Guid.NewGuid().ToString() }
                };

                var result = await userManager.CreateAsync(user, request.Password);

                if(result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(Roles.Admin))
                        await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                    if (!await roleManager.RoleExistsAsync(Roles.AppUser))
                        await roleManager.CreateAsync(new IdentityRole(Roles.AppUser));

                    if (await roleManager.RoleExistsAsync(Roles.Admin))
                    {
                        await userManager.AddToRoleAsync(user, Roles.Admin);
                    }

                    return new User()
                    {
                        Username = user.UserName,
                        ImageUrl = user.Photo.Url ?? ""
                    };
                }

                throw new Exception("Problem creating administrator");
            }
        }

    }
}
