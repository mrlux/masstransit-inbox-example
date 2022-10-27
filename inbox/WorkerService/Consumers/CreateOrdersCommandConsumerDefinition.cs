using MassTransit;
using WorkerService.Data;

namespace WorkerService.Consumers;

public class CreateOrdersCommandConsumerDefinition : ConsumerDefinition<CreateOrdersCommandConsumer>
{
    private readonly IServiceProvider _serviceProvider;

    public CreateOrdersCommandConsumerDefinition(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateOrdersCommandConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<WorkerDbContext>(_serviceProvider);
        if (endpointConfigurator is IServiceBusReceiveEndpointConfigurator cfg)
        {
            cfg.MaxAutoRenewDuration = TimeSpan.FromSeconds(20);
            cfg.LockDuration = TimeSpan.FromSeconds(2);
        }
    }
}