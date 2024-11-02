
using AutoMapper;
using BCrypt.Net;
using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models;
using EcommercePetsFoodBackend.Data.Models.Customer;
using EcommercePetsFoodBackend.Db_Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;


namespace EcommercePetsFoodBackend.Services.CustomerServices
{
    public class Customer : ICustomer
    {
        private readonly EcomContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public Customer(EcomContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> CustomerRegister(CustomerRegisterDto customers)
        {

            var customer = await _context.Customers.FirstOrDefaultAsync(u => u.Email == customers.Email);
            if (customer != null) {
                return "user already exists";
            }
            var custom = _mapper.Map<Customers>(customers);
            custom.Role = "user";
            custom.Password = BCrypt.Net.BCrypt.HashPassword(customers.Password);
            _context.Customers.Add(custom);
            _context.SaveChanges();
            return ("registration Succeessfull");
        }

        public async Task<LoginDto> CustomerLogin(CustomerLogin customers)
        {

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == customers.Email);
            if (customer == null)
            {

                return new LoginDto { Error = "User not found" };
            }
            var password = BCrypt.Net.BCrypt.Verify(customers.Password, customer.Password);
            if (password)
            {
                if (customer.IsBlocked)
                {
                    return new LoginDto { Error = "Blocked user" };
                }


                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
            new Claim(ClaimTypes.Name, customer.Name),
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.Role, customer.Role)
        }),
                    Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["JwtSettings:ExpiresInHours"])),
                    Issuer = _configuration["JwtSettings:Issuer"],
                    Audience = _configuration["JwtSettings:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return new LoginDto
                {
                    Email = customer.Email,
                    Token = jwtToken
                };
            }
            return new LoginDto { Error = "oopss!" };
        }

        public async Task<IEnumerable<AdminRegDto>> GetCustomers()
        {
            try
            {

                var customer = await _context.Customers.ToListAsync();
                var map = _mapper.Map<IEnumerable<AdminRegDto>>(customer);
                return map;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<AdminRegDto> GetCustomersById(int id)
        //{
        //    try
        //    {
        //        var customer=await _context.Customers.FindAsync(id);
        //        var map=_mapper.Map<AdminRegDto>(customer);
        //        return map;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        public async Task<AdminRegDto> GetCustomersById(int id)
        {
            try
            {
                var customer = await _context.Customers.FromSqlRaw("select * from Customers where Id=@Id ", new SqlParameter("@Id", id))
                   .FirstOrDefaultAsync();
                if (customer != null)
                {
                    return new AdminRegDto
                    {
                        name = customer.Name,
                        Email = customer.Email,
                        Role = customer.Role
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> BlockCustomer(string email)
        {
            try
            {
                var customer = await _context.Database.ExecuteSqlRawAsync("UPDATE Customers SET IsBlocked = 1 WHERE Email = @Email",
                new SqlParameter("@Email", email));
                return customer > 0;
            }
            
            catch (Exception ex)
            {
                throw new Exception("error in blocking cuustomer");
            }

        }

 
    }
}
    