using EksiSozluk.Api.Application.Features.Queries.GetUserDetails;
using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels.UserCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            UserDetailsViewModel user = await mediator.Send(new GetUserDetailsQuery(id));

            return Ok(user);
        }

        [HttpGet]
        [Route("UserName/{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            UserDetailsViewModel user = await mediator.Send(new GetUserDetailsQuery(Guid.Empty, userName));

            return Ok(user);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            LoginUserViewModel res = await mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            Guid guid = await mediator.Send(command);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            Guid guid = await mediator.Send(command);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Confirm")]
        public async Task<IActionResult> ConfirmEMail(Guid id)
        {
            bool guid = await mediator.Send(new ConfirmEmailCommand() { ConfirmationId = id });

            return Ok(guid);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            if (!command.UserId.HasValue)
                command.UserId = UserId;

            bool guid = await mediator.Send(command);

            return Ok(guid);
        }
    }
}
