using EcommercePetsFoodBackend.Data.Dto;

namespace EcommercePetsFoodBackend.Services.ServiceCart
{
    public interface ICartServices
    {
        Task <IEnumerable<CartDto>> GetAllItems(int id);
        Task<bool> AddCartItem (int id,int productid);
        Task<bool> DeleteCartItem (int id,int productid);
        Task<bool>IncrementQuantity(int id,int productid);
        Task<bool>DecrementQuantity(int id,int productid);
    }
}
