using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Products;
using EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePetsFoodBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _Product;
        public ProductController(IProductServices product)
        {
            _Product = product;
        }

        [HttpPost("AddProduct")]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult> AddProduct([FromBody] ProductDto product)
        {
            try
            {
                var newProduct =await _Product.AddProduct(product);
                return Ok(newProduct);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpPost("addingCategory")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddCategory([FromBody]string category)
        {
            try
            {
                var newCategoory = await _Product.AddNewCategory(category);
                return Ok(newCategoory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
    }
}
