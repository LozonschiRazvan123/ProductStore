using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ProductStore.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class MessageHub: Hub
    {
        private readonly IHubContext<MessageHub> _hubContext;
        public MessageHub(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendOffersToUser(List<string> message)

           => await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}
