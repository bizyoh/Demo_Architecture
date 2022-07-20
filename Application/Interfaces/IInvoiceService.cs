using Domain.Entities;
using Application.Models.DTO.InvoiceDto;

namespace Application.Interfaces.InvoiceService
{
    public interface IInvoiceService
    {
        public bool Create(CreateInvoiceDto createInvoiceDto);
        public List<Invoice> FindInvoiceByUserId(int id);
        public List<InvoiceDetailUserViewModel> GetAllInvoiceDetailUserViewModel(int id);
        public List<InvoiceUserViewModel> GetAllInvoiceUserViewModel();
        public List<InvoiceUserViewModel> GetAllInvoiceUserViewModelByUserId(int userId);
    }
}
