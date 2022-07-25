using Domain.Entities;
using Application.Models.DTO.Product;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.ProductService
{
    public interface IProductService
    {
        public List<GetAllProductDto> GetAllProducts();
        public Product GetById(int id);
        public DetailProductDto GetDetailProductDtoById(int id);
        public bool HardDelete(int id);
        public bool ChangeProductsStatus(List<Product> products);
        public CreatedProductDto Create(CreateProductDto createProductDto);
        public bool Update(int id, UpdateProductDto updateProductDto);
        public dynamic UploadPhoto(int id , IFormFile file);
        public List<GetAllProductDto> GetAllProductsDtoByStatus(bool status);
        public List<GetAllProductDto> GetAllProductsDtoTop3ByDate();
        public Task<ProductCartDto> GetProductCartDtoById(int id);
        public List<GetAllProductDto> GetAllProductsDtoByCategoryId(int categoryId);
        public List<ProductAdminViewModel> GetAllProductAdminViewModel();
        public bool ChangeProductStatus(int id);

    }
}
