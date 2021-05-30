using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Features.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {
        public ProfileController(IMediator mediator)
            : base(mediator) { }

        [HttpGet]
        [Route("favoritedrecipes")]
        [Authorize]
        public async Task<ActionResult<FavoritedRecipes.RecipesEnvelope>> FavoritedRecipes()
        {
            return await Mediator.Send(new FavoritedRecipes.Query());
        }
    }
}
