using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OrderImportQuiz
{
    class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        private string[] LaunchArgs { get; }
        
        public OrderContextFactory() { }
        public OrderContextFactory(string[] args)
        {
            LaunchArgs = args;
        }

        public OrderContext GetNewDbContext()
        {
            return CreateDbContext(LaunchArgs);
        }
        
        public OrderContext CreateDbContext(string[]? args = null)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder
                // Uncomment the following line if you want to print generated
                // SQL statements on the console.
                // .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new OrderContext(optionsBuilder.Options);
        }
    }
}