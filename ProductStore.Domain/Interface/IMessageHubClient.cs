using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(IList<string> message);
    }
}
