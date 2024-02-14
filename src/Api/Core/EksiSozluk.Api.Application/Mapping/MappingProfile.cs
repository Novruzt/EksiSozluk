using AutoMapper;
using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>().ReverseMap();
        }
    }
}
