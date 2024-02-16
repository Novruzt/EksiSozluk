using AutoMapper;
using EksiSozluk.Api.Domain.Models;
using EksiSozluk.Common.Models.Queries;
using EksiSozluk.Common.Models.RequestModels.EntryCommands;
using EksiSozluk.Common.Models.RequestModels.EntryCommentCommands;
using EksiSozluk.Common.Models.RequestModels.UserCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EksiSozluk.Api.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {

            CreateMap<User, LoginUserViewModel>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();  

            CreateMap<Entry, CreateEntryCommand>().ReverseMap();
            CreateMap<Entry, GetEntriesViewModel>()
                .ForMember(dest=>dest.CommentCount, opt=>opt.MapFrom(src=>src.EntryComments.Count)).ReverseMap();
            

            CreateMap<CreateEntryCommentCommand, EntryComment>().ReverseMap();
            
        }
    }
}
