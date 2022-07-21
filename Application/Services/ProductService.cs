using Application.Exceptions;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.CategoryService;
using Application.Interfaces.ProductService;
using Application.Models.DTO.Product;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class ProductService : IProductService
    {

        private IApplicationDbContext db;
        private IMapper mapper;
        private IWebHostEnvironment webHostEnvironment;
        private IHttpContextAccessor httpContextAccessor;
        private IConfiguration configuration;
        private ICategoryService categoryService;
        public ProductService( IMapper _mapper, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor _httpContextAccessor, IConfiguration _configuration, IApplicationDbContext _db)
        {
            db = _db;

            mapper = _mapper;
            webHostEnvironment = _webHostEnvironment;
            httpContextAccessor = _httpContextAccessor;
            configuration = _configuration;
        }
        public CreatedProductDto Create(CreateProductDto createProductDto)
        {
            var photo = UploadPhoto(createProductDto.File);
            var product = mapper.Map<Product>(createProductDto);
            product.Photo = photo;
            product.Created = DateTime.Now;
            db.Products.Add(product);
            if (db.SaveChanges() > 0)
            {
                CreatedProductDto createdProduct = mapper.Map<CreatedProductDto>(product);
                return createdProduct;
            }
            return null;
        }

        public bool HardDelete(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<GetAllProductDto> GetAllProducts()
        {
            return db.Products.Select(x => new GetAllProductDto
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                Price = x.Price,
                Photo = configuration["IMG_URL"] + "products/" + x.Photo,
            }).ToList();
        }

        public List<ProductAdminViewModel> GetAllProductAdminViewModel()
        {
            var products = db.Products.Select(x => new ProductAdminViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.CategoryId,
                Price = x.Price,
                Photo = configuration["IMG_URL"] + "products/" + x.Photo,
                Created = x.Created,
                Quantity = x.Quantity,
                Status = x.Status
            }).ToList();
            return products;
        }


        public List<Product> GetAllProductsByStatus(bool status)
        {
            return db.Products.Where(x => x.Status == status).ToList();
        }

        public Product GetById(int id)
        {
            return db.Products.Find(id);
        }

        public bool Update(int id, UpdateProductDto updateProductDto)
        {
            var currentProduct = db.Products.Find(id);
            if (currentProduct != null)
            {
                if (updateProductDto.Photo is null)
                {
                    updateProductDto.Photo = currentProduct.Photo;
                }
                mapper.Map(updateProductDto, currentProduct);

                db.Update(currentProduct);
                if (db.SaveChanges() > 0) return true;
            }
            throw new KeyNotFoundException(MessageErrors.ItemNotFound);

            return false;
        }


        public bool ChangeProductsStatus(List<Product> products)
        {
            foreach (var product in products)
            {
                db.Products.Find(product.Id);
                product.Status = !product.Status;
                db.SaveChanges();
            }
            return true;
        }

        public bool ChangeProductStatus(int id)
        {
            var product = db.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                product.Status = !product.Status;
                db.Update(product);
                return db.SaveChanges() > 0;
            }
            throw new AppException(MessageErrors.ItemNotFound);
        }
        public string UploadPhoto(IFormFile file)
        {
            string fileName = GenerateFileName(file);
            var path = Path.Combine(webHostEnvironment.WebRootPath, "images/products", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            var baseURL = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host + httpContextAccessor.HttpContext.Request.PathBase;
            return fileName;

        }

        public dynamic UploadPhoto(int id, IFormFile file)
        {
            if (db.Products.Find(id) == null) throw new KeyNotFoundException(MessageErrors.ItemNotFound);
            string fileName = GenerateFileName(file);

            var path = Path.Combine(webHostEnvironment.WebRootPath, "images/products", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                throw new KeyNotFoundException(MessageErrors.ItemNotFound);
            }
            else
            {
                string oldFileName = product.Photo;
                product.Photo = fileName;
                db.SaveChanges();
                DeleteFile(oldFileName);
            }
            var baseURL = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host + httpContextAccessor.HttpContext.Request.PathBase;
            return new
            {
                fileURL = baseURL + "/uploads/images/products" + file.FileName,
                fileSize = file.Length,
                fileName,
            };
        }

        public bool DeleteFile(string fileName)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "images/products", fileName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            return false;
        }

        public string GenerateFileName(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            string FileName = Guid.NewGuid().ToString().Replace("-", "");
            string ReturnValue = FileName + extension;
            if (!File.Exists(Path.Combine(webHostEnvironment.WebRootPath, ReturnValue)))
            {
                return ReturnValue;
            }
            return GenerateFileName(file);
        }

        public DetailProductDto GetDetailProductDtoById(int id)
        {
            var product = db.Products.FirstOrDefault(x => x.Id == id);
            var detailProductDto = mapper.Map<DetailProductDto>(product);
            return detailProductDto;
        }

        public List<GetAllProductDto> GetAllProductsDtoByStatus(bool status)
        {
            var products = db.Products.Where(x => x.Status == status).ToList();
            var getAllProductsDto = mapper.Map<List<Product>, List<GetAllProductDto>>(products);
            return getAllProductsDto;
        }

        public List<GetAllProductDto> GetAllProductsDtoTop3ByDate()
        {
            var products = db.Products.Where(x => x.Status == true).OrderByDescending(x => x.Id).Take(3).ToList();
            var getAllProductsDto = mapper.Map<List<Product>, List<GetAllProductDto>>(products);
            return getAllProductsDto;
        }
        public List<GetAllProductDto> GetAllProductsDtoByCategoryId(int categoryId)
        {
            var products = db.Products.Where(x => x.Status == true && x.CategoryId == categoryId).ToList();
            var getAllProductsDto = mapper.Map<List<Product>, List<GetAllProductDto>>(products);
            return getAllProductsDto;
        }

        public async Task<ProductCartDto> GetProductCartDtoById(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException(MessageErrors.ItemNotFound);
            var productCartDto = mapper.Map<ProductCartDto>(product);
            return productCartDto;
        }


    }
}
