using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Wishlists;
using EcommercePetsFoodBackend.Db_Context;
using Microsoft.EntityFrameworkCore;

namespace EcommercePetsFoodBackend.Services.WishlistServices
{
    public class WishlistServices:IwishlistServices
    {
        private readonly EcomContext _context;
        private readonly IConfiguration _configuration;
        public WishlistServices(EcomContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<WishlistDto>> GetWishlist(int id)
        {
            try
            {
                var user=await _context.Customers
                    .Include(x => x.Wishlist)
                    .ThenInclude(p=>p.product)
                    .FirstOrDefaultAsync(c=>c.Id == id);

                if(user == null)
                {
                    throw new Exception("no user found");
                }
                if(user.Wishlist == null || !user.Wishlist.Any())
                {
                    return new List<WishlistDto>();
                }
                var data = user.Wishlist.Select(p => new WishlistDto
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProductName = p.product.ProductName,
                    Description = p.product.ProductDescription,
                    price = ((decimal)p.product.Price),
                    categoryName = p.product.Category.CategoryName,
                    image = $"{_configuration["HostUrl:Images"]}/Products/{p.product.Image}"

                });
                return data.ToList();
                }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }



        public async Task<bool> AddWishlist(int id, int productId)
        {
            try
            {
                var custom = await _context.Customers
                    .Include(w => w.Wishlist)
                    .ThenInclude(p => p.product)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (custom == null)
                {
                    throw new Exception("user not found");
                }
                var item = custom.Wishlist.FirstOrDefault(p => p.ProductId == productId);
                if (item == null)
                {
                    custom.Wishlist.Add(new Wishlist
                    {
                        UserId = id,
                        ProductId = productId
                    });
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }


        public async Task<bool> DeleteWishlist(int wishlistId, int ProductId)
        {
            try
            {
                var data = await _context.Customers
                    .Include(w => w.Wishlist)
                    .ThenInclude(p => p.product)
                    .FirstOrDefaultAsync(u => u.Id == wishlistId);
                if (data == null)
                {
                    throw new Exception("user not found");
                }
                var item = data.Wishlist.FirstOrDefault(p => p.ProductId == ProductId);
                if (item == null)
                {
                    return false;
                }
                data.Wishlist.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }


        //        public async Task<bool> DeleteWishlist(int wishlistId, int ProductId)
        //        {
        //            try
        //            {

        //                var sql = @"if exists(
        //select 1 from wishlists 
        //where userId={0} and ProductId={1})
        //begin
        //delete from Wishlists
        //where UserId={0} and ProductId={1};
        //end";

        //                var affectedRows=await _context.Database.ExecuteSqlRawAsync(sql,wishlistId,ProductId);
        //                return affectedRows > 0;
        //            }
        //            catch (Exception ex)
        //            {
        //                var innerMessage = ex.InnerException?.Message ?? ex.Message;
        //                throw new Exception($"Database update error: {innerMessage}");
        //            }
        //        }



    }
}
