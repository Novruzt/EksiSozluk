using EksiSozluk.Common;
using EksiSozluk.Common.Events.EntryComment;
using EksiSozluk.Common.Infastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.EntryComment.CreateFav
{
    public class CreateEntryCommentFavCommandHandler : IRequestHandler<CreateEntryCommentFavCommand, bool>
    {
        public async Task<bool> Handle(CreateEntryCommentFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.FavExchangeName, 
                         exchangeType: SozlukConstant.DefaultExchangeType, 
                         queueName: SozlukConstant.CreateEntryCommentFavQueueName, 
                         obj: new CreateEntryCommentFavEvent()
                         {
                             EntryCommentId = request.EntryCommentId,
                             CreatedBy=request.UserId
                         });

            return await Task.FromResult(true);
        }
    }
}
