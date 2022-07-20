using Application.Interfaces.ProductService;
using Application.Models.DTO.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService productService;
        private IConfiguration config;
        private string BASE_URL;
        public ProductsController(IProductService _productService, IConfiguration _config)
        {
            productService = _productService;
            config = _config;
            BASE_URL = config["BASE_URL"];
        }


        [HttpGet("admin")]
        [AllowAnonymous]
        public IActionResult GetAllProductAdminViewModel()
        {
            var products = productService.GetAllProductAdminViewModel();
            return Ok(products);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllProduct()
        {
            var products = productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("categories/{id}")]
        [AllowAnonymous]
        public IActionResult GetAllProductsDtoByCategoryId(int id)
        {
            var products = productService.GetAllProductsDtoByCategoryId(id);
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("3/products")]
        public IActionResult GetAllProductTop3ByDate()
        {
            var products = productService.GetAllProductsDtoTop3ByDate();
            return Ok(products);
        }

        [AllowAnonymous]
        [HttpGet("status")]
        public IActionResult GetAllProductsDtoByStatus([FromQuery]string status)
        {
             var statusParse = bool.Parse(status);
            var products = productService.GetAllProductsDtoByStatus(statusParse);
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromForm]CreateProductDto createProductDto)
        {
            if (createProductDto != null)
            {
                var createdProduct = productService.Create(createProductDto);
                if (createdProduct != null) return Created(BASE_URL+"api/product",createdProduct);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,UpdateProductDto updateProductDto)
        {
            if (updateProductDto != null)
            {
                var result = productService.Update(id,updateProductDto);
                if (result) return Ok();
            }
            return BadRequest();
        }

     

        [HttpPost("{id}/uploadphoto")]
        public IActionResult UploadPhoto( int id,  IFormFile file)
        {
            productService.UploadPhoto(id, file);
            return Ok();
            return BadRequest(); 
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetDetailProductById(int id)
        {
            var product = productService.GetDetailProductDtoById(id);
            if (product == null) return NotFound();
            else
            {
                return Ok(product);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("cart/{id}")]
        public IActionResult GetProductCartDtoById(int id)
        {
            var product = productService.GetDetailProductDtoById(id);
            if (product == null) return NotFound();
            else
            {
                return Ok(product);
            }
            return BadRequest();
        }

       
        [HttpGet("{id}/status")]
        public IActionResult ChangeStatusProduct(int id)
        {
            var product = productService.GetDetailProductDtoById(id);
            if (product == null) return NotFound();
            else
            {
                if (productService.ChangeProductStatus(id))
                {
                    return Ok();
                }

            }
            return BadRequest();
        }
    }

}
