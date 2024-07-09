using AutoMapper;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common;
using EksiSozluk.Common.Events.User;
using EksiSozluk.Common.Infrastructure;
using EksiSozluk.Common.Infrastructure.Exceptions;
using EksiSozluk.Common.Models.RequestModels.UserCommands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await userRepository.GetSingleAsync(u => u.EmailAddress == request.EmailAddress);

            if (existUser is not null)
                throw new DatabaseValidationException("This email adress is already used.");

            var dbUser = mapper.Map<Domain.Models.User>(request);
            int rows = await userRepository.AddAsync(dbUser);

            if(rows > 0) 
            {
                UserEmailChangedEvent @event = new UserEmailChangedEvent()
                {
                    OldEmailAdress = null,
                    NewEmailAdress=dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstant.UserExchangeName,
                                                   exchangeType: SozlukConstant.DefaultExchangeType,
                                                   queueName: SozlukConstant.UserEmailChangedQueueName,
                                                   obj: @event);
            }

            return dbUser.Id;
        }
    }

}
