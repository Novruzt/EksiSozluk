using EksiSozluk.Api.Application.Features.Queries.GetEntries;
using EksiSozluk.Api.Application.Features.Queries.GetEntryComments;
using EksiSozluk.Api.Application.Features.Queries.GetEntryDetails;
using EksiSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using EksiSozluk.Api.Application.Features.Queries.GetUserEntries;
using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Models.Page;
using EksiSozluk.Common.Models.Queries;
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

        [HttpGet]
        public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery query)
        {
            List<GetEntriesViewModel> entries = await mediator.Send(query);

            return Ok(entries);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            GetEntryDetailsViewModel result = await mediator.Send(new GetEntryDetailsQuery(id, UserId));

            return Ok(result);
        }

        [HttpGet]
        [Route("Comments/{id}")]
        public async Task<IActionResult> GetEntryComments(Guid id, int page, int pageSize)
        {
            PagedViewModel<GetEntryCommentsViewModel> result = await mediator.Send(new GetEntryCommentsQuery(id, UserId, page, pageSize));

            return Ok(result);
        }

        [HttpGet]
        [Route("UserEntries")]
        [Authorize]
        public async Task<IActionResult> GetUserEntries(string userName, Guid userId, int page, int pageSize)
        {
            if (userId == Guid.Empty && string.IsNullOrEmpty(userName))
                userId = UserId.Value;

            PagedViewModel<GetUserEntriesDetailsViewModel> result = await mediator.Send(new GetUserEntriesQuery(userId, userName, page, pageSize));

            return Ok(result);
        }

        [HttpGet]
        [Route("MainPageEntries")]
        public async Task<IActionResult> GetMainPageEntries(int page, int pageSize)
        {
            PagedViewModel<GetEntryDetailsViewModel> entries = await mediator.Send(new GetMainPageEntriesQuery(UserId, page, pageSize));

            return Ok(entries);
        }

        [HttpPost]
        [Route("CreateEntry")]
        [Authorize]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            Guid result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpPost]
        [Route("CreateEntryComment")]
        [Authorize]
        public async Task<IActionResult> CreateEntryComment([FromBody] CreateEntryCommentCommand command)
        {
            if (!command.CreatedById.HasValue)
                command.CreatedById = UserId;

            Guid result = await mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        [Route("Search")]
        public async Task<IActionResult> Search([FromQuery] SearchEntryQuery query)
        {
            List<SearchEntryViewModel> result = await mediator.Send(query);

            return Ok(result);
        }

    }
}
