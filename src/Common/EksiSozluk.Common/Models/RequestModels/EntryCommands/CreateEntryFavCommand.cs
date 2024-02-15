using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Models.RequestModels.EntryCommands
{
    public class CreateEntryFavCommand:IRequest<bool>
    {
        public Guid? UserId { get; set; }
        public Guid? EntryId { get; set; }
        public CreateEntryFavCommand(Guid? userId, Guid? entryId)
        {
            UserId = userId;
            EntryId = entryId;
        }
    }
}
