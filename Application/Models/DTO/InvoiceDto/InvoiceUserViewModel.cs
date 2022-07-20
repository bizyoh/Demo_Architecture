namespace Application.Models.DTO.InvoiceDto
{
    public class InvoiceUserViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public virtual List<InvoiceDetailUserViewModel> invoiceDetailUserViewModels { get; set; }
    }
}
