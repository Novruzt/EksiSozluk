using EksiSozluk.Common;
using EksiSozluk.Common.Events.Entry;
using EksiSozluk.Common.Infastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.Entry.DeleteVote
{
    public class DeleteEntryVoteCommandHandler : IRequestHandler<DeleteEntryVoteCommand, bool>
    {
        public async Task<bool> Handle(DeleteEntryVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.VoteExchangeName,
                                               exchangeType: SozlukConstant.DefaultExchangeType,
                                               queueName: SozlukConstant.DeleteEntryVoteQueueName,
                                               obj: new DeleteEntryVoteEvent()
                                               {
                                                   EntryId= request.EntryId,
                                                   CreatedBy=request.UserId
                                               });

            return await Task.FromResult(true);
        }
    }
}
