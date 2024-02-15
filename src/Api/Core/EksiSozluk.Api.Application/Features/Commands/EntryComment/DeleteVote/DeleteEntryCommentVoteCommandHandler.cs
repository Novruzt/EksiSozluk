using EksiSozluk.Common;
using EksiSozluk.Common.Events.EntryComment;
using EksiSozluk.Common.Infastructure;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.EntryComment.DeleteVote
{
    public class DeleteEntryCommentVoteCommandHandler : IRequestHandler<DeleteEntryCommentVoteCommand, bool>
    {
        public async Task<bool> Handle(DeleteEntryCommentVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.DeleteEntryCommentVoteQueueName,
                                               exchangeType: SozlukConstant.DefaultExchangeType,
                                               queueName: SozlukConstant.DeleteEntryCommentVoteQueueName,
                                               obj: new DeleteEntryCommentVoteEvent()
                                               {
                                                   EntryCommentId= request.EntryCommentId,
                                                   CreatedBy=request.UserId
                                               });

            return await Task.FromResult(true);
        }
    }
}
