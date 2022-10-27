using MassTransit;
using Messages;

namespace WorkerService.Consumers;

public class CreateOrdersCommandConsumer : IConsumer<CreateOrdersCommand>
{
    public async Task Consume(ConsumeContext<CreateOrdersCommand> context)
    {
        var events = new List<OrderCreatedEvent>();
        for (int i = 0; i < context.Message.AmountToCreate; i++)
        {
            events.Add(new OrderCreatedEvent());
        }

        await context.PublishBatch(events);
    }
}