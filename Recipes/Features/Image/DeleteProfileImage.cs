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
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == currentUserAccessor.GetCurrentUsername());
                context.Images.Load();
                var photo = user.Photo.Id == request.Id ? user.Photo : null;

                if(photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                var result = await photoAccessor.DeletePhotoAsync(request.Id, context);

                if (!result)
                    throw new Exception("Problem deleting photo");

                return Unit.Value;
            }
        }

    }
}
