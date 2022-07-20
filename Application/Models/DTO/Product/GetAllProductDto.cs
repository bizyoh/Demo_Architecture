namespace Application.Models.DTO.Product
{
    public class GetAllProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }
        public int CategoryId { get; set; }
    }
}
