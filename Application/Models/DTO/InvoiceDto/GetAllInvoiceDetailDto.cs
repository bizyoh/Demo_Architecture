namespace Application.Models.DTO.InvoiceDto
{
    public class GetAllInvoiceDetailDto
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public double Amount { get; set; }
    }
}
