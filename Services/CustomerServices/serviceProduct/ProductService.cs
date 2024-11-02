using AutoMapper;
using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Products;
using EcommercePetsFoodBackend.Db_Context;
using Microsoft.EntityFrameworkCore;

namespace EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct
{
    public class ProductService:IProductServices
    {
        private readonly EcomContext _context;
        private readonly IMapper _mapper;

        public ProductService(EcomContext context)
        {
            _context = context;
        }

       

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            try
            {
                var data = await _context.Products.FromSqlRaw("select p.* from products p inner join categories c on p.categoryId=c.Id").ToListAsync();
                if (data != null)
                {
                    return _mapper.Map<IEnumerable<ProductDto>>(data);
                }
                return null;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }






    }
}
