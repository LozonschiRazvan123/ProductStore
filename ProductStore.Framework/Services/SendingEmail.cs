using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using ProductStore.Interface;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Framework.Services
{
    public class SendingEmail : IJob
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
            var customerEmails = await _customer.GetCustomers();

            var emailDTOs = customerEmails.Select(email => new EmailDTO
            {
                To = email.Email,
                Subject = "Message",
                Body = "This is a message"
            });

            foreach (var emailDTO in emailDTOs)
            {
                _emailService.SendEmail(emailDTO);
            }

            Console.WriteLine("DADADD");
        }
    }
}
