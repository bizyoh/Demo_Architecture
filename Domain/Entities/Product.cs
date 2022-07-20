namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
