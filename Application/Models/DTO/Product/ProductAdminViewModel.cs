namespace Application.Models.DTO.Product
{
    public class ProductAdminViewModel
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }
        public int CategoryId { get; set; }
    }
}
