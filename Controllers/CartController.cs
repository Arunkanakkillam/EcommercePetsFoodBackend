using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Services.ServiceCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommercePetsFoodBackend.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;
        public CartController(ICartServices cartservices)
        {
            _cartServices = cartservices;
        }
        [HttpGet("cartitems")]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetAllItems()
        {
            try
            {
                var user_id = GetUserId();
                var _user_id = user_id.Value;
                var data = await _cartServices.GetAllItems(_user_id);

                if (data == null||!data.Any()) 
                {
                    return NotFound("no user");
                }
                if (data.Count() == 0)
                {
                    return BadRequest("no item in cart");
                }
                return Ok(data);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("additemtocart/{productid}")]
        public async Task<IActionResult> AddCartItem(int productid)
        {
            try
            {
                var user_id = GetUserId();
                var _user_Id = user_id.Value;
                var data = await _cartServices.AddCartItem(_user_Id, productid);
                if (data)
                {
                    return Ok("successfull!!");
                }
                return BadRequest("Item already in cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("cartitem delete/{productid}")]
        public async Task<IActionResult> DeleteCartItem(int productid)
        {
            try
            {
                var user_id = GetUserId();
                var _user_id = user_id.Value;
                var data = await _cartServices.DeleteCartItem(_user_id, productid);
                if (data)
                {
                    return Ok("item deleted");
                }
                return NotFound("No product found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("increment product/{productid}")]
        public async Task<IActionResult> IncrementQuantity(int productid)
        {
            try
            {
                var user_id = GetUserId();
                var _user_id=user_id.Value;
                var data=await _cartServices.IncrementQuantity(_user_id, productid);
                if (data)
                {
                    return Ok("uantity incresed");
                }
                return NotFound("no product");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Decrement product/{productid}")]
        public async Task<IActionResult> DecrementQuantity(int productid)
        {
            try
            {
                var user_id = GetUserId();
                var _user_id = user_id.Value;
                var data = await _cartServices.DecrementQuantity(_user_id, productid);
                if (data)
                {
                    return Ok("uantity incresed");
                }
                return NotFound("no product");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private ActionResult<int> GetUserId()
        {
            var IdInString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(IdInString, out var id))
            {
                return id;
            }
            return Unauthorized();
        }
    }
}
