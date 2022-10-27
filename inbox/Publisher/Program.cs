// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Azure.Identity;
using MassTransit;
using MassTransit.AzureServiceBusTransport.Configuration;
using Messages;
using Microsoft.Extensions.Configuration;



var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .Build();

var busControl = Bus.Factory.CreateUsingAzureServiceBus((cfg) =>
{
    cfg.Host(new HostSettings
    {
        ServiceUri = new Uri(configuration.GetConnectionString("AzureServiceBus")),
        TokenCredential = new DefaultAzureCredential()
    });
    cfg.Publish<CreateOrdersCommand>();
});

var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

await busControl.StartAsync(source.Token);

Console.WriteLine("Press enter to publishing the CreateOrdersCommand");
Console.ReadLine();
await busControl.Publish(new CreateOrdersCommand(30000));
Console.WriteLine("CreateOrdersCommand published");