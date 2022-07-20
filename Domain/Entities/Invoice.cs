namespace Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public virtual User User { get; set; }
        public virtual List<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
