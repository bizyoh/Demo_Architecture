using AutoMapper;
using Domain.Entities;
using Application.Models.DTO.Product;

namespace Infrastructure.Files.Maps
{
    public class ProductMapper : Profile
    {
        private string BASE_URL = "https://localhost:7023/images/products/";
        public ProductMapper()
        {
            CreateMap<UpdateProductDto, Product>().ForAllMembers(opts => opts.Condition((src, des, srcMem) => srcMem != null || srcMem != ""));
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, CreatedProductDto>();
            CreateMap<Product, UpdateProductDto>();
            CreateMap<Product, DetailProductDto>().ForMember(x => x.Photo, opt => opt.MapFrom(src => BASE_URL + src.Photo));
            CreateMap<Product, GetAllProductDto>().ForMember(x => x.Photo, opt => opt.MapFrom(src => BASE_URL + src.Photo));
            CreateMap<Product, ProductCartDto>().ForMember(x => x.Photo, opt => opt.MapFrom(src => BASE_URL + src.Photo));
        }
    }
}
