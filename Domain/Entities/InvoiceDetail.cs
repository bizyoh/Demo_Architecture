namespace Domain.Entities
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
