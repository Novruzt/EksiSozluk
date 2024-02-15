using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Models.RequestModels.EntryCommands
{
    public class CreateEntryCommand:IRequest<Guid>
    {
        public Guid? CreatedById { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public CreateEntryCommand()
        {
            
        }
        public CreateEntryCommand(Guid? createdById, string subject, string content)
        {
            CreatedById = createdById;
            Subject = subject;
            Content = content;
        }
    }
}
