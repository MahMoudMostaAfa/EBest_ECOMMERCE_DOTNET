using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EBest.Models
{
    public class Product
    {
        public int Id { set; get; }

        [MaxLength(100)]
        public string Name { set; get; } = "";

        [MaxLength(50)]
        public string Category { set; get; } = "";

        [Precision(16, 2)]
        public decimal Price { set; get; }

        [MaxLength(50)] 
        public string Brand { set; get; } = "";

        public string Description { set; get; } = "";
        [MaxLength(200)]
        public string ImageFileName { set; get; } = ""; 

        public DateTime CreatedAt { set; get; } = DateTime.Now; 



    }
}
