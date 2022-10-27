using Azure.Identity;
using MassTransit;
using MassTransit.AzureServiceBusTransport.Configuration;
using Microsoft.EntityFrameworkCore;
using WorkerService;
using WorkerService.Consumers;
using WorkerService.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder,services) =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<WorkerDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));
        
        services.AddMassTransit(x =>
        {
            
            x.AddConsumer<CreateOrdersCommandConsumer, CreateOrdersCommandConsumerDefinition>();

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(new HostSettings
                {
                    ServiceUri = new Uri(builder.Configuration.GetConnectionString("AzureServiceBus")),
                    TokenCredential = new DefaultAzureCredential()
                });
                cfg.UseRetry(r => r.Intervals(5, 10, 30, 60, 600));
                cfg.ConfigureEndpoints(context);
            });

            x.AddEntityFrameworkOutbox<WorkerDbContext>(o =>
            {
                o.UseSqlServer();
                o.UseBusOutbox();
            });
        });
    })
    .Build();


await using(var scope = host.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WorkerDbContext>();
    dbContext.Database.EnsureCreated();
}
await host.RunAsync();