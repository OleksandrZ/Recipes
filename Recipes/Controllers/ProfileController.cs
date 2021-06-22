using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain;
using Recipes.Features.UserProfile;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : BaseController
    {
        public ProfileController(IMediator mediator)
            : base(mediator) { }

        [HttpPost]
        [Route("addprofileimage")]
        public async Task<ActionResult<Photo>> AddProfileImage([FromForm] AddProfileImage.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Route("deleteprofileimage")]
        public async Task<ActionResult<Unit>> DeleteProfileImage(DeleteProfileImage.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("favoritedrecipes")]
        public async Task<ActionResult<FavoritedRecipes.RecipesEnvelope>> FavoritedRecipes()
        {
            return await Mediator.Send(new FavoritedRecipes.Query());
        }
    }
}
