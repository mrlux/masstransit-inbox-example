using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace WorkerService.Data;

public class WorkerDbContext : DbContext
{
    public WorkerDbContext(DbContextOptions<WorkerDbContext> options): base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        const string outboxSchemaName = "outbox";
        modelBuilder.AddInboxStateEntity(o => o.ToTable("InboxState", outboxSchemaName));
        modelBuilder.AddOutboxMessageEntity(o => o.ToTable("OutboxMessage", outboxSchemaName));
        modelBuilder.AddOutboxStateEntity(o => o.ToTable("OutboxState", outboxSchemaName));
    }
}