using EksiSozluk.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Domain.Models
{
    public class EntryVote:BaseEntity
    {
        public Guid EntryId { get; set; }
        public VoteType VoteType { get; set; }
        public Guid CreatedById { get; set; }


        //reliations
        public virtual Entry Entry { get; set; }

    }
}
