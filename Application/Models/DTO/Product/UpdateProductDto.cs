using Application.Models.DTO.Category;

namespace Application.Models.DTO.Product
{
    public class UpdateProductDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string? Photo { get; set; }
        public int CategoryId { get; set; }

       
    }
}
