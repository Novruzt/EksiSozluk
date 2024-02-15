using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : BaseController
    {
        private readonly IMediator mediator;

        public EntryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("CreateEntry")]
        [Authorize]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            var result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateEntryComment")]
        [Authorize]
        public async Task<IActionResult> CreateEntryComment([FromBody] CreateEntryCommentCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            var result = await mediator.Send(command);

            return Ok(result);
        }
    }
}
