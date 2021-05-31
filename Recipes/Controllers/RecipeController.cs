using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Features.DTOs;
using Recipes.Features.Recipes;
using Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : BaseController
    {
        public RecipeController(IMediator mediator)
            : base(mediator) { }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List.RecipesEnvelope>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet]
        [Route("{id}")]
        //pass recipe id to get recipe's details
        public async Task<ActionResult<RecipeDto>> Details(string id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        // receives multipart/form-data, converts key command (json) to class Command
        public async Task<ActionResult<Unit>> Create([ModelBinder(BinderType = typeof(JsonModelBinder))] Create.Command command,IFormFile mainImage, ICollection<IFormFile> images)
        {
            command.Images = images;
            command.MainImage = mainImage;
            return await Mediator.Send(command);
        }

        [Authorize(Policy = "IsAuthor")]
        [HttpPut]
        [Route("edit")]
        //pass recipe id to edit recipe
        public async Task<ActionResult<Unit>> Edit(Edit.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = "IsAuthor")]
        [HttpDelete]
        [Route("delete")]
        //pass recipe id to delete recipe
        public async Task<ActionResult<Unit>> Delete(Delete.Command command)
        {
            return await Mediator.Send(command);
        }

        //pass recipe id to add to favorite
        [Authorize]
        [HttpPost("{id}/addfavorite")]
        public async Task<ActionResult<Unit>> AddToFavorite(string id)
        {
            return await Mediator.Send(new AddToFavorite.Command { Id = id });
        }

        //pass recipe id to remove from favorite
        [HttpDelete("{id}/removefavorite")]
        [Authorize]
        public async Task<ActionResult<Unit>> RemoveFromFavorite(string id)
        {
            return await Mediator.Send(new RemoveFromFavorite.Command { Id = id });
        }
    }
}
