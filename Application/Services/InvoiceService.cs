using Application.Exceptions;
using Application.Filters;
using Application.Interfaces.InvoiceService;
using Application.Models.DTO.InvoiceDto;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private AppDBContext db;
        private IMapper mapper;
        public InvoiceService(AppDBContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        public List<InvoiceUserViewModel> GetAllInvoiceUserViewModel()
        {
            var invoices = db.Invoices.Select(p => new InvoiceUserViewModel
            {
                Id = p.Id,
                Created = p.Created,
                UserId = p.UserId
            }).ToList();
            return invoices;
        }

        public List<InvoiceDetailUserViewModel> GetAllInvoiceDetailUserViewModel(int id)
        {
            if (db.Invoices.Find(id) == null) throw new KeyNotFoundException(MessageErrors.ItemNotFound);
            var invoiceDetails = db.InvoiceDetails.Where(p => p.InvoiceId == id).Select(p => new InvoiceDetailUserViewModel
            {
                Amount = p.Amount,
                ProductId = p.ProductId,
                ProductName = p.Product.Name
            }).ToList();
            return invoiceDetails;
        }

        public bool Create(CreateInvoiceDto createInvoiceDto)
        {
            var invoice = mapper.Map<Invoice>(createInvoiceDto);
            var invoiceDetails = mapper.Map<List<InvoiceDetail>>(createInvoiceDto.CreateInvoiceDetailDtos);
            foreach (var invoiceDetail in invoiceDetails)
            {
                var product = db.Products.Find(invoiceDetail.ProductId);
                product.Quantity -= invoiceDetail.Amount;
                if (product.Quantity < 0)
                {
                    throw new AppException(MessageErrors.ProductOutOfStock);
                }
                db.Update(product);
            }
            invoice.InvoiceDetails = invoiceDetails;
            if (invoice.InvoiceDetails.Count > 0)
            {
                db.Invoices.Add(invoice);
            }
            return db.SaveChanges() > 0;
        }

        public List<Invoice> FindInvoiceByUserId(int id)
        {
            return db.Invoices.Where(invoice => invoice.UserId == id).ToList();
        }

        public List<InvoiceUserViewModel> GetAllInvoiceUserViewModelByUserId(int userId)
        {
            if (db.Users.Find(userId) is null)
            {
                throw new KeyNotFoundException(MessageErrors.NotFound);
            }
            var invoices = db.Invoices.Where(x => x.UserId == userId).Select(p => new InvoiceUserViewModel
            {
                Id = p.Id,
                Created = p.Created,
                UserId = p.UserId
            }).ToList();
            return invoices;

        }
    }
}
