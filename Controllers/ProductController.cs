using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Products;
using EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<ActionResult> AddProduct([FromForm] ProductDto product,IFormFile img)
        {
            try
            {
                var newProduct =await _Product.AddProduct(product,img);
                return Ok(newProduct);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("Get All Products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            try
            {
                var data=await _Product.GetAllProducts();
                return Ok(data);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductsById/{id}")]
      
        public async Task<ActionResult> GetProductById(int id)
        {
            try
            {
                var productById=await _Product.GetProductById(id);
                return Ok(productById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update_product")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult>UpdateProduct(int id, [FromForm]ProductDto product,[FromForm]IFormFile img)
        {
            try
            {
                if (id == 0 || product == null) 
                {
                    return BadRequest("please fill all  the fields");
                }
                var updatedProduct=await _Product.UpdateProduct(id, product,img);
                if (updatedProduct)
                {
                    return Ok(updatedProduct);
                }
                return NotFound();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductsByCategoryId/{id}/{pageno}/{pagesize}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByCategoryId(int id, int pageno, int pagesize)
        {
            try
            {
                var data=await _Product.GetProductByCategoryId(id, pageno, pagesize);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpDelete("deleteProduct/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var delt = await _Product.DeleteProduct(id);
                return Ok($"successfully deleted{id}");
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

        [HttpDelete("DeleteCategory/{id}")]
        [Authorize(Roles = "admin")]
    public async Task<IActionResult>DeleteCategory(int id)
        {
            try
            {
                await _Product.DeleteCategory(id);
                return Ok($"succcessfully deleted{id}");
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("searchItem")]
        public async Task<IActionResult> SearchProduct(string SearchItem)
        {
            try
            {
                if (SearchItem == null)
                {
                    return BadRequest("please write something");
                }
                var result=_Product.SearchProduct(SearchItem);

                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
