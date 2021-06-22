using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipes.Features.User;
using System;
using System.Threading.Tasks;

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
            query.Ip = IpAddress();

            var user = await Mediator.Send(query);

            SetTokenCookie(user.RefreshToken);

            return user;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<User>> RefreshToken(Features.User.RefreshToken.Query query)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            query.Token = refreshToken;
            query.Ip = IpAddress();

            var user = await Mediator.Send(query);

            SetTokenCookie(user.RefreshToken);

            return user;
        }

        [HttpPost]
        [Route(("logout"))]
        [Authorize]
        public async Task<Unit> Logout(Logout.Command command)
        {
            var token = Request.Cookies["refreshToken"];

            command.Token = token;
            command.Ip = IpAddress();

            return await Mediator.Send(command);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<User>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register-admin")]
        public async Task<ActionResult<User>> RegisterAdmin(RegisterAdmin.Command command)
        {
            return await Mediator.Send(command);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}