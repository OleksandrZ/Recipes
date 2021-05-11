using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Recipes.Domain;
using Recipes.Features.Difficulty;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyController : BaseController
    {
        public DifficultyController(IMediator mediator)
            : base(mediator) { }


        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<string>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

    }
}
