using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.Features.User;
using System.Threading.Tasks;
using MediatR;

namespace Recipes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator)
            : base(mediator) { }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<ActionResult<User>> RegisterAdmin(RegisterAdmin.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}