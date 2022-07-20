namespace Application.Models.DTO.InvoiceDto
{
    public class CreateInvoiceDto
    {
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public virtual List<CreateInvoiceDetailDto> CreateInvoiceDetailDtos { get; set; }
    }
}
