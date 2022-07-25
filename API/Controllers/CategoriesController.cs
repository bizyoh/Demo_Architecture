using Application.Exceptions;
using Application.Filters;
using Application.Interfaces.CategoryService;
using Application.Models.DTO.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
 //   [Authorize(Roles="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService categoryService;
        private IConfiguration configuration;
        private string BASE_URL;
        public CategoriesController(ICategoryService _categoryService, IConfiguration _configuration)
        {
            categoryService = _categoryService;
            configuration = _configuration;
        }

        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var categories = categoryService.GetAllCategory();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetDetailCategoryDtoById(int id)
        {
            var detailCategoryDto = categoryService.GetDetailCategoryDtoById(id);
            return Ok(detailCategoryDto);
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public IActionResult GetAllCategoryByStatus([FromQuery]bool status)
        {
            var categories = categoryService.GetAllCategoryByStatus(status);
            return Ok(categories);
        }


        [HttpPost]
        public IActionResult Create(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null) throw new AppException(MessageErrors.CategoryInvalid);
            else
            {
                var createdCategory = categoryService.Create(createCategoryDto);
                if (createdCategory != null) return Created(BASE_URL + "/category/" + createdCategory.Id, createdCategory);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,[FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (categoryService.GetById(id) == null) return NotFound();
            if (updateCategoryDto != null)
            {
                var result = categoryService.Update(id, updateCategoryDto);
                if (result) return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            var category = categoryService.GetById(id);
            if (category == null) return NotFound();
            else
            {
                var result = categoryService.Delete(id);
                if (result) return Ok();
            }
            return BadRequest();
        }
    }
}
