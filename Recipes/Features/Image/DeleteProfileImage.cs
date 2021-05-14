using MediatR;
using Microsoft.EntityFrameworkCore;
using Recipes.Infrastructure;
using Recipes.Infrastructure.Errors;
using Recipes.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Features.Image
{
    public static class DeleteProfileImage
    {

        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {

            private readonly RecipesDbContext context;
            private ICurrentUserAccessor currentUserAccessor;
            private IPhotoAccessor photoAccessor;

            public Handler(RecipesDbContext context, ICurrentUserAccessor currentUserAccessor, IPhotoAccessor photoAccessor)
            {
                this.context = context;
                this.currentUserAccessor = currentUserAccessor;
                this.photoAccessor = photoAccessor;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await context.Images.LoadAsync(cancellationToken: cancellationToken);
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername(), cancellationToken: cancellationToken);

                if(user.Photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Photo doesn't exist" });

                if(user.Photo.Id != request.Id)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Photo doesn't exist" });

                var photo = user.Photo;

                var result = await photoAccessor.DeletePhotoAsync(photo);

                if (!result)
                    throw new Exception("Problem deleting photo");

                return Unit.Value;
            }
        }

    }
}
