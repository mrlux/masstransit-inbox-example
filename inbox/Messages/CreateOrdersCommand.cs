namespace Messages;

public class CreateOrdersCommand
{
    public CreateOrdersCommand(int amountToCreate)
    {
        AmountToCreate = amountToCreate;
    }

    public int AmountToCreate { get; }
}