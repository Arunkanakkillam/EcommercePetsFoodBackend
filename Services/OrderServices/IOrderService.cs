using EcommercePetsFoodBackend.Data.Dto;

namespace EcommercePetsFoodBackend.Services.OrderServices
{
    public interface IOrderService
    {
        //Task<IEnumerable<OutPutOrderDto>> CustomersOrders(int id);
        //Task<IEnumerable<OutPutOrderDto>> GetAllOrders();
        //Task<bool> DeleteAllOrders(int id);
        Task<string> OrderCreate(long price);
        //Task<IEnumerable<object>> NumbersOfPurchase();
        //Task<decimal> TotalRevenue();
        Task <bool> Payment(RazorPayDto PaymentDto);
        Task<bool> CreateOrder(InputOrderDto UserCredential);
    }
}
