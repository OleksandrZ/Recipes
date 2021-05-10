using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain;
using Recipes.Features.Cuisines;
using Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineController : BaseController
    {
        public CuisineController(IMediator mediator)
            : base(mediator) { }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Cuisine>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        [Route("delete")]
        public async Task<ActionResult<Unit>> Delete(Delete.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List.CuisinesEnvelope>> List()
        {
            return await Mediator.Send(new List.Query());
        }
    }
}
