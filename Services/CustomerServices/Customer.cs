
using EcommercePetsFoodBackend.Data.Models;
using EcommercePetsFoodBackend.Db_Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace EcommercePetsFoodBackend.Services.CustomerServices
{
    public class Customer:ICustomer
    {
       private readonly EcomContext _context;
     private List<Customers> customers=new List<Customers>();
        public Customer(EcomContext context)
        {
            _context = context;
        }

        public async  Task<string> CustomerRegister(Customers customers)
        {
            var customer=await _context.Customers.FirstOrDefaultAsync(u=>u.Id==customers.Id);
            if (customer != null) {
                return "user already exists";
            }

            _context.Customers.Add(customer);
            _context.SaveChanges(); 
            return ("registration Succeessfull");
        }

        public async Task<Customers> CustomerLogin(Customers customers)
        {

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == customers.Email);

            throw new NotImplementedException();
        }
    }
}
