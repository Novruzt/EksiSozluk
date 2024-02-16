using EksiSozluk.Common.Models.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetUserDetails;
public class GetUserDetailsQuery:IRequest<UserDetailsViewModel>
{
    public GetUserDetailsQuery(Guid userId, string userName=null)
    {
        UserId = userId;
        UserName = userName;
    }

    public Guid UserId { get; set; }
    public string UserName { get; set; }
}
