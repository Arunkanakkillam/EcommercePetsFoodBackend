using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Customer;
using EcommercePetsFoodBackend.Services.CustomerServices;
using EcommercePetsFoodBackend.Services.WishlistServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EcommercePetsFoodBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
  
    public class WishlistController : ControllerBase
    {
        private IwishlistServices _wishlistService;
        public WishlistController(IwishlistServices wishlistService)
        {
            _wishlistService = wishlistService;
        }


       


        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistDto>>> GetWishlist()
        {
            try
            {
                var user_id=GetUserId();
                var _user_id=user_id.Value;
                if (_user_id==null)
                {
                    return Unauthorized("user not found");
                }
                var result = await _wishlistService.GetWishlist(_user_id);
                if (result == null)
                {
                    return NotFound("no user in this id");
                }
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addWishlist/{productId}")]
        public async Task<IActionResult> AddWishlist(int productId)
        {
            try
            {
                var user_id = GetUserId();
                var _user_id = user_id.Value;
                var result=await _wishlistService.AddWishlist(_user_id, productId);
                if (!result)
                {
                    return BadRequest("some problem iccured");
                }
                return Ok("successfully added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveWishlist{ProductId}")]
        public async Task<IActionResult> DeleteWishlist(int ProductId)
        {
            try
            {
                var user_id = GetUserId();
                var _user_id = user_id.Value;
                var result=await _wishlistService.DeleteWishlist(_user_id, ProductId);
                if (!result)
                {
                    return BadRequest("issue in removing product from wishlist");
                }
                return Ok("successfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



       private ActionResult<int> GetUserId()
        {
            var IdInString=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(int.TryParse(IdInString, out var id))
            {
                return id;
            }
            return Unauthorized();
        }
    }
}
