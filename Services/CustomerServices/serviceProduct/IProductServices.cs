using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Products;

namespace EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductByCategoryId(int id);
        Task<ProductDto> AddProduct(ProductDto product,IFormFile image);
        Task<bool> UpdateProduct(int id, ProductDto product,IFormFile image);
        Task <bool> DeleteProduct(int id);
        Task<bool> AddNewCategory(string category);
        Task<bool> DeleteCategory(int categoryId);
        Task<IEnumerable<ProductDto>> SearchProduct(string SearchItem);
    }
}
