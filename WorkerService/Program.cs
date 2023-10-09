using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using ProductStore.Framework.Services;
using WorkerService;
using WorkerService.Interface;
using WorkerService.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddSingleton<EmailDTO, EmailDTO>();
    })
    .Build();

await host.RunAsync();
