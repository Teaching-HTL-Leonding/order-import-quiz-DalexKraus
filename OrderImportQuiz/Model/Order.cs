using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderImportQuiz
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        
        [Column(TypeName = "decimal(8,2)")]
        public decimal OrderValue { get; set; }
    }
}