using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models;

namespace EcommercePetsFoodBackend.Services.CustomerServices
{
    public interface ICustomer
    {
        Task<string>CustomerRegister(CustomerRegisterDto customers);
        Task<LoginDto>CustomerLogin(CustomerLogin customers);
        Task<IEnumerable<AdminRegDto>> GetCustomers();
        Task<AdminRegDto> GetCustomersById(int id);
        Task <bool> BlockCustomer(string email);

    }
}
