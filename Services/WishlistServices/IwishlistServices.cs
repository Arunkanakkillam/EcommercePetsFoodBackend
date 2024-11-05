using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Customer;

namespace EcommercePetsFoodBackend.Services.WishlistServices
{
    public interface IwishlistServices
    {
        Task<IEnumerable<WishlistDto>> GetWishlist(int id);
        Task<bool> AddWishlist(int id, int productId);
        Task<bool> DeleteWishlist(int wishlistId, int ProductId);

    }
}
