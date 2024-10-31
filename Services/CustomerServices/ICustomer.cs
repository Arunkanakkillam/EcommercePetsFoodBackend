using EcommercePetsFoodBackend.Data.Models;

namespace EcommercePetsFoodBackend.Services.CustomerServices
{
    public interface ICustomer
    {
        Task<string>CustomerRegister(Customers customers);
        Task<Customers>CustomerLogin(Customers customers);

    }
}
