using AutoMapper;
using Domain.Entities;
using Application.Models.DTO.Category;

namespace Infrastructure.Files.Maps
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CreateCategoryDto,Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, CreatedCategoryDto>();
            CreateMap<Category, DetailCategoryDto>();
        }
    }
}
