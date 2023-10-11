using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using ProductStore.Interface;
using ProductStore.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Service
{
    public class SendingEmail: IJob
    {
        private readonly ILogger<SendingEmail> _logger;
        private readonly ICustomerRepository _customer;
        private readonly IEmailService _emailService;

        public SendingEmail(ILogger<SendingEmail> logger, ICustomerRepository customer, IEmailService emailService)
        {
            _logger = logger;
            _customer = customer;
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("DADADD");
            var customerEmails = await _customer.GetCustomers();

            foreach (var email in customerEmails)
            {
                var emailDTO = new EmailDTO
                {
                    To = email.Email,
                    Subject = "Message",
                    Body = "This is a message"
                };

                _emailService.SendEmail(emailDTO);
            }
        }
    }
}
