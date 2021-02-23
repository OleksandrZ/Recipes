using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Recipes.Domain;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Image
{
    public static class AddProfileImage
    {

        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly RecipesDbContext context;
            private ICurrentUserAccessor currentUserAccessor;
            private IPhotoAccessor photoAccessor;
            private readonly UserManager<AppUser> userManager;
            public Handler(ICurrentUserAccessor currentUserAccessor, RecipesDbContext context, IPhotoAccessor photoAccessor, UserManager<AppUser> userManager)
            {
                this.currentUserAccessor = currentUserAccessor;
                this.context = context;
                this.photoAccessor = photoAccessor;
                this.userManager = userManager;
            }
            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = photoAccessor.CreatePhoto(request.File);

                var user = await userManager.FindByNameAsync(currentUserAccessor.GetCurrentUsername());

                user.Photo = result;

                context.Images.Add(user.Photo);

                await context.SaveChangesAsync();

                return user.Photo;
            }
        }

    }
}
