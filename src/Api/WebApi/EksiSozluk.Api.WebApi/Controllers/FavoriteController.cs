using EksiSozluk.Api.Application.Features.Commands.Entry.DeleteFav;
using EksiSozluk.Api.Application.Features.Commands.EntryComment.CreateFav;
using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : BaseController
    {
        private readonly IMediator mediator;

        public FavoriteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("entry/{entryId}")]
        public async Task<IActionResult> CreateEntryFav(Guid entryId)
        {
            var result = await mediator.Send(new CreateEntryFavCommand(UserId, entryId));

            return Ok(result);
        }

        [HttpPost]
        [Route("entrycomment/{entrycommentId}")]
        public async Task<IActionResult> CreateEntryCommentFav(Guid entrycommentId)
        {
            var result = await mediator.Send(new CreateEntryCommentFavCommand(entrycommentId, UserId.Value));

            return Ok(result);
        }


        [HttpPost]
        [Route("deleteentryfav/{entryId}")]
        public async Task<IActionResult> DeleteEntryFav(Guid entryId)
        {
            var result = await mediator.Send(new DeleteEntryFavCommand(entryId, UserId.Value));

            return Ok(result);
        }

        [HttpPost]
        [Route("deleteentrycommentfav/{entrycommentId}")]
        public async Task<IActionResult> DeleteEntryCommentFav(Guid entrycommentId)
        {
            var result = await mediator.Send(new DeleteEntryCommentFavCommand(entrycommentId, UserId.Value));

            return Ok(result);
        }
    }
}
