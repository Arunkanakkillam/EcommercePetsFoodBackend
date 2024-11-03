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

        public ProductService(EcomContext context, IMapper mapper)
        {
            _context = context;
            _mapper=mapper;
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

        public async Task<ProductDto> AddProduct(ProductDto product)
        {
            try
            {
                var data = _mapper.Map<Product>(product);
                await _context.Products.AddAsync(data);
                await _context.SaveChangesAsync();
                return _mapper.Map<ProductDto>(data);
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }
        public async Task<bool> AddNewCategory(string category)
        {
            try
            {
                var data=await _context.Categories.AddAsync(new Category { CategoryName=category});
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
