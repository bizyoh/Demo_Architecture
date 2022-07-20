using Domain.Entities;

namespace Application.Models.DTO.InvoiceDto
{
    public class GetAllInvoiceDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public virtual List<InvoiceDetail> GetAllInvoiceDetailDtos{ get; set; }
    }
}
