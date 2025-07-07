using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBest.Models
{
    public class Order
    {
       
        public int Id { get; set; }
        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        public ApplicationUser Client { get; set; }

        public List<OrderItem> Items { get; set; }


        [Precision(16,2)]
        public decimal ShippingFee { get; set; }

        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public string PaymentDetails { get; set; } = ""; // to store paypal details
        public string OrderStatus { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
