using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Hubs
{
    public class CommentHub: Hub
    {
        public async Task SendMessage(/*string user,*/ string message)
        {
            await Clients.All.SendAsync("CommentMessage"/*, user*/, message);
        }
    }
}
