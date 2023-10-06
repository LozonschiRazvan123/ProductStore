using WorkerService;
using WorkerService.Interface;
using WorkerService.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
    })
    .Build();

await host.RunAsync();
