using EksiSozluk.Common.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Models.RequestModels.EntryCommands
{
    public class CreateEntryVoteCommand:IRequest<bool>
    {
        public Guid EntryId { get; set; }
        public Guid CreatedBy { get; set; }

        public VoteType VoteType { get; set; }

        public CreateEntryVoteCommand()
        {
            
        }
        public CreateEntryVoteCommand(Guid entryId,  VoteType voteType, Guid createdBy)
        {
            EntryId = entryId;
            CreatedBy = createdBy;
            VoteType = voteType;
        }
    }
}
