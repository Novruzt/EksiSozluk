using AutoMapper;
using EksiSozluk.Api.Application.Interfaces.Repositories;
using EksiSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetUserDetails;
public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsViewModel>
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public GetUserDetailsQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    public async Task<UserDetailsViewModel> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        Domain.Models.User dbUser = null;

        if (request.UserId != Guid.Empty)
            dbUser = await userRepository.GetByIdAsync(request.UserId);
        else if (!string.IsNullOrEmpty(request.UserName))
            dbUser = await userRepository.GetSingleAsync(i => i.UserName == request.UserName);
        else
            throw new ArgumentNullException("No id or username provided");
        

        return mapper.Map<UserDetailsViewModel>(dbUser);
    }
}
