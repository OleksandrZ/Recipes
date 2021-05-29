using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Recipes.Features.Comments;
using List = Recipes.Features.Comments.List;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : BaseController
    {
        public CommentsController(IMediator mediator)
            : base(mediator) { }

        [AllowAnonymous]
        [HttpPost]
        [Route("all")]
        public async Task<ActionResult<List<CommentDto>>> List(List.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<CommentDto>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = "IsAuthor")]
        [HttpPut]
        [Route("edit")]
        public async Task<ActionResult<Unit>> Edit(Edit.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = "IsAuthor")]
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<Unit>> Delete(Delete.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
