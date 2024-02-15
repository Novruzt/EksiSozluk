using EksiSozluk.Common;
using EksiSozluk.Common.Events.Entry;
using EksiSozluk.Common.Events.EntryComment;
using EksiSozluk.Common.Infastructure;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.Entry.CreateFav
{
    public class CreateEntryFavCommandHandler : IRequestHandler<CreateEntryFavCommand, bool>
    {
        public Task<bool> Handle(CreateEntryFavCommand request, CancellationToken cancellationToken)
        {
            QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.FavExchangeName,
                                               exchangeType: SozlukConstant.DefaultExchangeType,
                                               queueName: SozlukConstant.CreateEntryFavQueueName,
                                               obj: new CreateEntryFavEvent()
                                               {
                                                   EntryId=request.EntryId.Value,
                                                   CreatedBy=request.UserId.Value
                                               });

            return Task.FromResult(true);
        }
    }
}
