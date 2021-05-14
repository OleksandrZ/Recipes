using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain;
using Recipes.Features.Categories;
using Recipes.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator)
            : base(mediator) {}

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Category>> Create(Create.Command command)
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
        public async Task<ActionResult<List.CategoriesEnvelope>> List()
        {
            return await Mediator.Send(new List.Query());
        }
    }
}
