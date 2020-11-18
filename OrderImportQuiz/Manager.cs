using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrderImportQuiz
{
    public class Manager
    {
        private OrderContextFactory ContextFactory { get; }

        public Manager(string[] launchArgs)
        {
            ContextFactory = new OrderContextFactory(launchArgs);
        }

        public async Task ImportFromFile(string customerFile, string orderFile)
        {
            OrderContext context = ContextFactory.GetNewDbContext();
            var customersLines = await File.ReadAllLinesAsync(customerFile);
            var orderLines = await File.ReadAllLinesAsync(orderFile);

            foreach (var customer in customersLines.Skip(1))
            {
                var name = customer.Split("\t")[0];
                var limit = decimal.Parse(customer.Split("\t")[1]);
                var customerInstance = new Customer {Name = name, CreditLimit = limit};
                await context.Customers.AddAsync(customerInstance);

                var orders = new List<Order>();
                foreach (var order in orderLines.Skip(1))
                {
                    var splitLine = order.Split("\t");
                    if (!splitLine[0].Equals(customerInstance.Name)) continue;

                    var newOrder = new Order
                    {
                        OrderDate = DateTime.Parse(splitLine[1]),
                        OrderValue = int.Parse(splitLine[2]),
                        Customer = customerInstance
                    };
                    orders.Add(newOrder);
                    await context.Orders.AddAsync(newOrder);
                }
                customerInstance.Orders = orders;
            }
            
            await context.SaveChangesAsync();
        }

        public async Task Purge()
        {
            var context = ContextFactory.CreateDbContext();
            context.Customers.RemoveRange(context.Customers);
            context.Orders.RemoveRange(context.Orders);
            await context.SaveChangesAsync();
        }        
        
        public async Task Check()
        {
            var context = ContextFactory.GetNewDbContext();
            var customers = await context.Customers.ToListAsync();
            var orders = await context.Orders.ToListAsync();
            
            foreach (var customer in customers)
            {
                var orderSum = orders.Where(c => customer.Id == c.CustomerId).Sum(o => o.OrderValue);
                if (orderSum > customer.CreditLimit)
                    Console.WriteLine($"{customer.Name}: Exceeded CreditLimit by {(orderSum - customer.CreditLimit)}");
            }
        }
    }
}