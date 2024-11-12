using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Services.OrderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EcommercePetsFoodBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet("getallorders")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllorders()
        {
            try
            {
                var orderList = await _orderService.GetAllOrders();
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("userOrderadmin/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CustomersOrder(int id)
        {
            try
            {
                var order_List = await _orderService.CustomersOrders(id);
                return Ok(order_List);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("userorderuser")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UserOrders()
        {
            try
            {
                if (Convert.ToInt32(HttpContext.Items["Id"]) == null)
                {
                    return BadRequest();
                }
                var User_id = Convert.ToInt32(HttpContext.Items["Id"]);

                var order_list = await _orderService.CustomersOrders(User_id);

                return Ok(order_list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("place-order")]
        [Authorize(Roles ="user")]
        public async Task<ActionResult> PlaceOrder(InputOrderDto credentials)
        {
            try
            {
                var User_id = Convert.ToInt32(HttpContext.Items["Id"]);
                if (credentials == null || User_id != credentials.User_Id)
                {
                    return BadRequest();
                }
                var status = await _orderService.CreateOrder(credentials);
                return Ok(status);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("order-create")]
        [Authorize(Roles ="user")]
        public async Task<ActionResult> OrderCreate([FromBody] long price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest();
                }
                var orer_id = await _orderService.OrderCreate(price);
                return Ok(orer_id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("payment")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> Payment(RazorPayDto pay)
        {
            try
            {
                if (pay == null)
                {
                    return BadRequest("razopay details is not valid");
                }
                var userid = Convert.ToInt32(HttpContext.Items["Id"]);
                if (userid == null)
                {
                    return BadRequest();
                }
                var razor = _orderService.Payment(pay);
                return Ok(razor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("cancelOrder")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> CancelOrder()
        {
            try
            {
                var userid = Convert.ToInt32(HttpContext.Items["Id"]);
                if (userid == null)
                {
                    return BadRequest();
                }
                bool cancelled = await _orderService.DeleteAllOrders(userid);
                return cancelled ? Ok(cancelled) : BadRequest(cancelled);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("revenue")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Revenue()
        {
            try
            {
                var revenue=await _orderService.TotalRevenue();
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
