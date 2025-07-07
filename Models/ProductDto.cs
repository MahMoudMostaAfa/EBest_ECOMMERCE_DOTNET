using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EBest.Models
{
    public class ProductDto
    {
        [Required,MaxLength(100)]
        public string Name { set; get; } = "";

        [Required,MaxLength(50)]
        public string Category { set; get; } = "";

        [Required]
        public decimal Price { set; get; }

        [Required,MaxLength(50)]
        public string Brand { set; get; } = "";
        [Required]
        public string Description { set; get; } = "";
        
        public IFormFile? ImageFile { set; get; } 
    }
}
