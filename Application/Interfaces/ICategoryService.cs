using Domain.Entities;
using Application.Models.DTO.Category;


namespace Application.Interfaces.CategoryService
{
    public interface ICategoryService
    {
        public List<GetAllCategoryDto> GetAllCategory();
        public DetailCategoryDto GetDetailCategoryDtoById(int id);
        public List<CategoryDto> GetAllCategoryDtoByStatus(bool status);
        public List<GetAllCategoryDto> GetAllCategoryByStatus(bool status);
        public CreatedCategoryDto Create(CreateCategoryDto createCategoryDto);
        public bool Update(int id,UpdateCategoryDto updateCategoryDto);
        public bool Delete(int id);
        public Category GetById(int id);
    }
}
