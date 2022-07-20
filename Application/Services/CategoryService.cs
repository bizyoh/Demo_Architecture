
using Application.Exceptions;
using Application.Filters;
using Application.Interfaces.CategoryService;
using Application.Models.DTO.Category;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private AppDBContext db;
        private IMapper mapper;
        public CategoryService(AppDBContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        public CreatedCategoryDto Create(CreateCategoryDto createCategoryDto)
        {
            var category = mapper.Map<Category>(createCategoryDto);
            if (db.Categories.FirstOrDefault(x => x.Name == category.Name) != null)
            {
                throw new AppException(MessageErrors.UniqueCategory);
            }
            db.Categories.Add(category);
            if (db.SaveChanges() > 0)
            {
                var createdCategoryDto = mapper.Map<CreatedCategoryDto>(category);
                return createdCategoryDto;
            }
            return null;
        }

        public bool Delete(int id)
        {
            var category = db.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                db.Categories.Remove(category);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public List<GetAllCategoryDto> GetAllCategoryByStatus(bool status)
        {
            var categories = db.Categories.Where(x => x.Status == status).Select(x => new GetAllCategoryDto
            {
                Name = x.Name,
                Id = x.Id,
                Status = x.Status
            }).ToList();
            return categories;
        }

        public List<GetAllCategoryDto> GetAllCategory()
        {
            var categories = db.Categories.Select(x => new GetAllCategoryDto
            {
                Name = x.Name,
                Id = x.Id,
                Status = x.Status
            }).ToList();
            return categories;
        }

        public Category GetById(int id)
        {
            return db.Categories.Find(id);
        }

        public bool Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = db.Categories.Find(id);
            if (category == null) throw new KeyNotFoundException(MessageErrors.ItemNotFound);
            else
            {
                mapper.Map(updateCategoryDto, category);
                if (!CheckCategoryByName(id, category.Name))
                {
                    throw new AppException(MessageErrors.UniqueCategory);
                }
                if (category.Status == false)
                {
                    var products = db.Products.Where(x => x.CategoryId == category.Id).ToList();
                    foreach (var product in products)
                    {
                        product.Status = false;
                        db.Update(product);
                    }
                }
                db.Update(category);
                return db.SaveChanges() > 0;
            }
            return false;
        }

        public List<CategoryDto> GetAllCategoryDtoByStatus(bool status)
        {
            var categoryDtos = db.Categories.Where(x => x.Status == status).Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return categoryDtos;
        }

        public DetailCategoryDto GetDetailCategoryDtoById(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null) throw new KeyNotFoundException(MessageErrors.ItemNotFound);
            var detailCategoryDto = mapper.Map<DetailCategoryDto>(category);
            return detailCategoryDto;
        }

        public bool CheckCategoryByName(int id, string name)
        {
            var cates = db.Categories.Where(x => x.Id != id).ToList();
            foreach (var cate in cates)
            {
                if (cate.Name == name) return false;
            }
            return true;
        }
    }
}
