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
    public class MessageHub: Hub<IMessageHubClient>
    {
        public async Task SendOffersToUser(string user, List<string> message)

           => await Clients.All.SendOffersToUser(user, message);
    }
}
