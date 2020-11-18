using Microsoft.EntityFrameworkCore;

namespace OrderImportQuiz
{
    public class OrderContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        { }
    }
}