﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Features.Recipes;
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
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List.RecipesEnvelope>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<RecipeDto>> Details(Details.Query query)
        {
            return await Mediator.Send(query);
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
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
