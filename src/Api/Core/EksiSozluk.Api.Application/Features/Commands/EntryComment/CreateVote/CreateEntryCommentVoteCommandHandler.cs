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

namespace EksiSozluk.Api.Application.Features.Commands.EntryComment.CreateVote
{
    public class CreateEntryCommentVoteCommandHandler : IRequestHandler<CreateEntryCommentVoteCommand, bool>
    {
        public async Task<bool> Handle(CreateEntryCommentVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.CreateEntryCommentVoteQueueName,
                                               exchangeType: SozlukConstant.DefaultExchangeType,
                                               queueName: SozlukConstant.CreateEntryCommentVoteQueueName,
                                               obj: new CreateEntryCommentVoteEvent()
                                               {
                                                   VoteType=request.VoteType,
                                                   EntryCommentId=request.EntryCommentId,
                                                   CreatedBy=request.CreatedBy,
                                               });


            return await Task.FromResult(true);
        }
    }
}
