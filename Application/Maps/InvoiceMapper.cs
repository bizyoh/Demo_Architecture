using AutoMapper;
using Domain.Entities;
using Application.Models.DTO.InvoiceDto;

namespace Infrastructure.Files.Maps
{
    public class InvoiceMapper : Profile
    {
        public InvoiceMapper()
        {
            CreateMap<CreateInvoiceDto, Invoice>();
            CreateMap<CreateInvoiceDetailDto, InvoiceDetail>();
            CreateMap<Invoice, GetAllInvoiceDto>();
        }
    }
}
