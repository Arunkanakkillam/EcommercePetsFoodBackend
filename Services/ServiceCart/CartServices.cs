using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.cartmodel;
using EcommercePetsFoodBackend.Db_Context;
using Microsoft.EntityFrameworkCore;

namespace EcommercePetsFoodBackend.Services.ServiceCart
{
    public class CartServices:ICartServices
    {
        private readonly EcomContext _context;
        private readonly IConfiguration _configuration;
        public CartServices(EcomContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IEnumerable<CartDto>> GetAllItems(int id)
        {
            try
            {
                var data =await _context.Customers
                    .Include(c => c.cart)
                    .ThenInclude(c => c.Items)
                    .ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(u=>u.Id==id);
                if (data == null)
                {
                    throw new Exception("uesr not found");
                }
                if (data.cart == null||!data.cart.Items.Any())
                {
                    return Enumerable.Empty<CartDto>();
                }
                var items = data.cart.Items.Select(p=>new CartDto
                {
                    Id=p.Id,
                    Title = p.Product.ProductName,
                    Description = p.Product.ProductDescription,
                    Image = $"{_configuration["HostUrl:Images"]}/Products/{p.Product.Image}",
                    Price=p.Product.Price,
                    Quantity=p.Product.Quandity,
                    Total=p.Quantity * p.Product.Price
                });
                return items.ToList();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }

        public async Task<bool> AddCartItem(int id, int productid)
        {
            try
            {
                var data=await _context.Customers.Include(c=>c.cart)
                    .ThenInclude(c=>c.Items)
                    .ThenInclude(c=>c.Product)
                    .FirstOrDefaultAsync(u=>u.Id==id);
                if (data == null)
                {
                    throw new Exception("user not found");
                }
                if(data.cart == null)
                {
                    data.cart = new Cart { UserId = id, Items = new List<CartItem>() };
                    await _context.Carts.AddAsync(data.cart);
                    await _context.SaveChangesAsync();
                }
                var item = data.cart.Items.FirstOrDefault(c=>c.ProductId== productid);
                if (item != null) 
                {
                    return false;
                }
                var cartitem=new CartItem { CartId = data.cart.Id,ProductId=productid,Quantity=1 };
                data.cart.Items.Add(cartitem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }

        public async Task<bool> DeleteCartItem(int id, int productid)
        {
            try
            {
                var data=await _context.Customers.Include(c=>c.cart)
                    .ThenInclude (c=>c.Items)
                    .ThenInclude(c=>c.Product)
                    .FirstOrDefaultAsync(u=>u.Id==id);
                if (data == null)
                {
                    throw new Exception("user not found");
                }
                var item=data.cart.Items.FirstOrDefault(p=>p.ProductId==productid);
                if (item == null) 
                {
                    return false ;
                }
                 data.cart.Items.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }
        public async Task<bool> IncrementQuantity(int id, int productid)
        {
            try
            {
                var data=await _context.Customers.Include(c=>c.cart)
                    .ThenInclude(c=>c.Items)
                    .ThenInclude(p=>p.Product)
                    .FirstOrDefaultAsync(u=>u.Id==id);  
                if(data==null)
                {
                    throw new Exception("user not found");
                }
                var item=data.cart.Items.FirstOrDefault(u=>u.ProductId==productid);
                if (item == null) 
                {
                    return false ;
                }
                item.Quantity += 1;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }

        public async Task<bool> DecrementQuantity(int id, int productid)
        {
            try
            {
                var data = await _context.Customers.Include(c => c.cart)
                    .ThenInclude(c => c.Items)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (data == null)
                {
                    throw new Exception("user not found");
                }
                var item = data.cart.Items.FirstOrDefault(u => u.ProductId == productid);
                if (item == null)
                {
                    return false;
                }
                if (item.Quantity <= 1) 
                {
                    return false;
                }
                item.Quantity -= 1;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }


    }
}
