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
    public sealed class MessageHub: Hub
    {
        public async Task SendOffersToUser(List<string> message)

           => await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
