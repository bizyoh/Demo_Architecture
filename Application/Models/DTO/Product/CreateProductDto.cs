using Microsoft.AspNetCore.Http;

namespace Application.Models.DTO.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public IFormFile File { get; set; }
    }
}
