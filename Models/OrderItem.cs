using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBest.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        [Precision(16, 2)]
        public decimal UnitPrice { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
