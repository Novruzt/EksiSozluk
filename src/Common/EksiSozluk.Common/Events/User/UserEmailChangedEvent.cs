using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Common.Events.User
{
    public class UserEmailChangedEvent
    {
       public string OldEmailAdress {get; set;}
       public string NewEmailAdress { get; set; }
    }
}
