using Microsoft.EntityFrameworkCore;
using ProductStore.Core.DTO;
using ProductStore.Core.Interface;
using ProductStore.Data;
using ProductStore.Framework.Services;
using ProductStore.Interface;
using ProductStore.Repository;
using Quartz;
using System.Configuration;
using WorkerService;
using WorkerService.Interface;
using WorkerService.Service;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddHostedService<Worker>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IBackgroundTaskQueue, BackgroundTaskQueue>();
        services.AddScoped<ICustomerRepository, CostumerRepository>();
        services.AddTransient<Seed>();
    })

     .ConfigureServices((hostContext, services) =>
     {
         services.AddDbContext<DataContext>(options =>
         {
             options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
         });
         

         // Unique key for the job
         JobKey jobKey = new JobKey("my-job");

         services.AddQuartz(q =>
         {
             q.UseMicrosoftDependencyInjectionJobFactory();
             // Add each job's configuration like this
             q.AddJob<SendingEmail>(config => config.WithIdentity(jobKey));
             // Add each job's trigger like this and bind it to the job by the key
             q.AddTrigger(config => config
                 .WithIdentity("my-job-trigger")
                 .WithCronSchedule("0/5 * * * * ?")// la fiecare 5 secunde face SendingEmail.cs
                 .ForJob(jobKey));
         });
         // Let Quartz know that it should finish all the running jobs slowly before shutting down.
         services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
     })
    .Build();

await host.RunAsync();
