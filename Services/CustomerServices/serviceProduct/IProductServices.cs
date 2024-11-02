using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Products;

namespace EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        //Task<Product> GetProductById(int id);
        //Task<Product> GetProductByCategoryId(int id);
        //Task<ProductDto> AddProduct(ProductDto product);
        //Task<Product> UpdateProduct(Product product); 
        //Task<Product> DeleteProduct(int id);
        //Task<Product>AddNewCategory(Product product);
        //Task<Product>DeleteCategory(int categoryId);
        //Task<Product> SearchProduct(string SearchItem);
    }
}
