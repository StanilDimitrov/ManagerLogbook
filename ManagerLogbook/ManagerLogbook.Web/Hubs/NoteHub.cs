using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerLogbook.Web.Hubs
{
    public class NoteHub : Hub
    {
        public async Task CommentAdded(int id, string CommentText)
        {
            await Clients.All.SendAsync("ReceiveNote", id, CommentText);
        }
    }
}
