using ProductStore.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Core.Interface
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
