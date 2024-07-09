using EksiSozluk.Common;
using EksiSozluk.Common.Events.EntryComment;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.EntryComment.DeleteFav
{
    public class DeleteEntryCommentFavCommandHandler : IRequestHandler<DeleteEntryCommentFavCommand, bool>
    {
        public async Task<bool> Handle(DeleteEntryCommentFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.DeleteEntryCommentFavQueueName,
                                               exchangeType: SozlukConstant.DefaultExchangeType,
                                               queueName: SozlukConstant.DeleteEntryCommentFavQueueName,
                                               obj: new DeleteEntryCommentFavEvent()
                                               {
                                                   EntryCommentId= request.EntryCommentId,
                                                   CreatedBy=request.UserId
                                               });


            return await Task.FromResult(true);
        }
    }
}
