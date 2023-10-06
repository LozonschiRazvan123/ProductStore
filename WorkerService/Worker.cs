using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using WorkerService.Interface;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEmailService _email;
        private readonly IBackgroundTaskQueue _taskQueue;

        public Worker(ILogger<Worker> logger, IEmailService email, IBackgroundTaskQueue taskQueue)
        {
            _logger = logger;
            _email = email;
            _taskQueue = taskQueue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now;

                if (currentTime.Hour == 14 && currentTime.Minute == 0)
                {
                    var task = await _taskQueue.DequeueAsync(stoppingToken);

                    if (task != null)
                    {
                        var emailDTO = new EmailDTO();
                        _email.SendEmail(emailDTO);
                        await task(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Verifica?i coada la fiecare minut
                }
            }
        }
    }
}