using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.Interface;
using ProductStore.Models;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using WorkerService.Interface;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _email;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ICustomerRepository _customer;
        private readonly BackgroundService _backgroundService;


        public Worker(ILogger<Worker> logger, IEmailService email, IBackgroundTaskQueue taskQueue, ICustomerRepository customer, BackgroundService backgroundService)
        {
            _logger = logger;
            _email = email;
            _taskQueue = taskQueue;
            _customer = customer;
            _backgroundService = backgroundService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _backgroundService.StartAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var currentTime = DateTimeOffset.Now;

                if (currentTime.Hour == 14 && currentTime.Minute == 00)
                {
                    
                    var customerEmails = await _customer.GetCustomers();

                    foreach (var email in customerEmails)
                    {
                        var emailDTO = new EmailDTO
                        {
                            To = email.Email,
                            Subject = "Subiect e-mail",
                            Body = "Corpul e-mailului"
                        };

                        _email.SendEmail(emailDTO);
                    }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
    }


}