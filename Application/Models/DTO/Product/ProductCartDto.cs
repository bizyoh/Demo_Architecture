namespace Application.Models.DTO.Product
{
    public class ProductCartDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int Quantity { get; set; }
        private double Price { get; set; }
    }
}
