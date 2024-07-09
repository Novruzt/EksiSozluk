using EksiSozluk.Common;
using EksiSozluk.Common.Events.Entry;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.Entry.CreateVote
{
    internal class CreateEntryVoteCommandHandler : IRequestHandler<CreateEntryVoteCommand, bool>
    {
        public async Task<bool> Handle(CreateEntryVoteCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.VoteExchangeName,
                                               exchangeType: SozlukConstant.DefaultExchangeType,
                                               queueName: SozlukConstant.CreateEntryVoteQueueName,
                                               obj: new CreateEntryVoteEvent()
                                               {
                                                   EntryId = request.EntryId,
                                                   CreatedBy=request.CreatedBy,
                                                   VoteType = request.VoteType,
                                               });

            return await Task.FromResult(true);
        }
    }
}
