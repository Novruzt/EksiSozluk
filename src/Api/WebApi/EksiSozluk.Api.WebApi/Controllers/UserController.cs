using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
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

            LoginUserViewModel response = await mediator.Send(command);

            return Ok(response);

        }
    }
}
