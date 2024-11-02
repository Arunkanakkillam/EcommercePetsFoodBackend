using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models;
using EcommercePetsFoodBackend.Services.CustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePetsFoodBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customer;

        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }

        [HttpPost]
        public async Task<ActionResult> CustomerRegister([FromBody] CustomerRegisterDto customerRegisterDto)
        {
            try
            {
                var isExist = await _customer.CustomerRegister(customerRegisterDto);
                return Ok(isExist);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> CustomerLogin([FromBody] CustomerLogin loginDto)
        {
            try
            {
                var existingCustomer = await _customer.CustomerLogin(loginDto);
                if (existingCustomer == null)
                {
                    return NotFound("check username or password");
                }
                if(existingCustomer.Error=="Blocked user")
                {
                    return BadRequest("user is blocked");
                }

                if (existingCustomer.Error=="oopss!")
                {
                    return BadRequest("password doesn't match");
                }
                return Ok(new LoginDto { Token=existingCustomer.Token});
            }
            catch(Exception ex) 
            {
                return StatusCode (500,ex.Message);
            }
        }


        [HttpGet("customers")]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<IEnumerable<AdminRegDto>>> GetCustomers()
        {
            try
            {
                var existingCustomer = await _customer.GetCustomers();
                return Ok(existingCustomer);
            }
            catch(Exception ex) 
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("CustomersById/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetCustomersById(int id)
        {
            try
            {
                var existingCustomer = await _customer.GetCustomersById(id);
                return Ok(existingCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut("block-Customer")]
        [Authorize(Roles = "admin")]    
        public async Task<ActionResult> BlockCustomer([FromBody] string email)
        {
            try
            {
                var done=await _customer.BlockCustomer(email);
                return Ok(done);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

    }
}
