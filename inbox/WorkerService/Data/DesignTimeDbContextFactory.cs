using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkerService.Data;

public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<WorkerDbContext>
{
    public WorkerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .Build();
        
        var options = new DbContextOptionsBuilder<WorkerDbContext>();
        options.UseSqlServer(configuration.GetConnectionString("Sql"));
        
        return new WorkerDbContext(options.Options);
    }
}