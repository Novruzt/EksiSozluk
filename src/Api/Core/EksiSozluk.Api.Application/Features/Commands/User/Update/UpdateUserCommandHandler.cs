using AutoMapper;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common;
using EksiSozluk.Common.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksiSozluk.Common.Models.RequestModels.UserCommands;

namespace EksiSozluk.Api.Application.Features.Commands.User.Update
{
    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);

            if (dbUser is null)
                throw new DatabaseValidationException("User not found.");

            string dbEmailAddress= dbUser.EmailAddress;
            bool emailChanged=string.CompareOrdinal(dbEmailAddress, request.EmailAddress) !=0; 

            mapper.Map(request, dbUser);
            int rows = await userRepository.UpdateAsync(dbUser);

            if (emailChanged && rows > 0)
            {
                UserEmailChangedEvent @event = new UserEmailChangedEvent()
                {
                    OldEmailAdress = dbEmailAddress,
                    NewEmailAdress = request.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.UserExchangeName,
                                                   exchangeType: SozlukConstant.DefaultExchangeType,
                                                   queueName: SozlukConstant.UserEmailChangedQueueName,
                                                   obj: @event);


                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}
