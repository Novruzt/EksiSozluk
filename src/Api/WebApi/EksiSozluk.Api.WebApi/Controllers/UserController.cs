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

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var res = await mediator.Send(command);

            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            var guid = await mediator.Send(command);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var guid = await mediator.Send(command);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Confirm")]
        public async Task<IActionResult> ConfirmEMail(Guid id)
        {
            var guid = await mediator.Send(new ConfirmEmailCommand() { ConfirmationId = id });

            return Ok(guid);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            if (!command.UserId.HasValue)
                command.UserId = UserId;

            var guid = await mediator.Send(command);

            return Ok(guid);
        }
    }
}
