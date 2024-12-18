﻿using AutoMapper;
using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Products;
using EcommercePetsFoodBackend.Db_Context;
using EcommercePetsFoodBackend.Migrations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EcommercePetsFoodBackend.Services.CustomerServices.serviceProduct
{
    public class ProductService:IProductServices
    {
        private readonly EcomContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public ProductService(EcomContext context, IMapper mapper,IWebHostEnvironment webHostEnvironment, IConfiguration configuration )
        {
            _context = context;
            _mapper=mapper;
            _webHostEnvironment= webHostEnvironment;
            _configuration = configuration;
        }

       

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            try
            {
                var data = await _context.Products.FromSqlRaw("select p.* from products p inner join categories c on p.productCategoryId=c.categoryId").ToListAsync();
                if (data != null)
                {
                    var img = data.Select(p =>
                    new ProductDto
                    {
                        ProductId = p.ProductId,
                        IsAvailable = p.IsAvailable,
                        Image = $"{_configuration["HostUrl:Images"]}/Products/{p.Image}",
                        Price = p.Price,
                        ProductCategoryId = p.ProductCategoryId,
                        ProductDescription = p.ProductDescription,
                        ProductName = p.ProductName,
                        Quandity = p.Quandity
                    });
                    return img.ToList();
                }
                return null;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductDto> AddProduct(ProductDto product, IFormFile image)
        {
            try
            {
                var data = _mapper.Map<Product>(product);
                if (image != null && image.Length > 0) 
                {
                    var FileName=Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var FilePath=Path.Combine(_webHostEnvironment.WebRootPath,"Images","Products",FileName);
                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    data.Image = FileName;
                }
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

        public async Task<Product> GetProductById(int id)
        {
            try
            {
                var data = await _context.Products.FromSqlRaw("select * from Products where ProductId=@Id", new SqlParameter("@Id", id)).FirstOrDefaultAsync();
                if(data != null)
                {
                    return _mapper.Map<Product>(data);
                }
                return null;

            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }

        }
        public async Task<IEnumerable<ProductDto>> GetProductByCategoryId(int id,int pageno,int pagesize)
        {
            try
            {
                if(pageno <= 0)
                {
                    pageno = 1;
                }
                if (pagesize <= 0) 
                {
                    pagesize = 2;
                }
                int skip = (pageno - 1) * pagesize;

                var data = await _context.Products.FromSqlRaw("select * from Products where ProductCategoryId=@Id", new SqlParameter("@Id", id))
                    .Skip(skip)
                    .Take(pagesize)
                    .ToListAsync();
                if(data != null)
                {
                    var product = data.Select(p => new ProductDto
                    {
                        ProductId = p.ProductId,
                        Image = $"{_configuration["HostUrl:Images"]}/Products/{p.Image}",
                        Price = p.Price,
                        ProductCategoryId = p.ProductCategoryId,
                        ProductDescription = p.ProductDescription,
                        ProductName = p.ProductName,
                        Quandity = p.Quandity
                    });

                    return product;
                }
                return Enumerable.Empty<ProductDto>();

            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }
        public async Task<bool> UpdateProduct(int id,ProductDto product,IFormFile image)
        {
            try
            {
                var data = await _context.Products.FirstOrDefaultAsync(p => p.ProductId==id);
                if (data == null)
                {
                    return false;
                }
                int SomeId;
                if (!int.TryParse(product.ProductCategoryId.ToString(), out SomeId)) 
                {
                    throw new Exception("invalid category id");
                }
                string SomeImage=null;
                if (image != null && image.Length > 0) 
                {
                    var FileName=Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var DirectoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products");
                    if (!Directory.Exists(DirectoryPath)) 
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }
                    var FilePath=Path.Combine(DirectoryPath,FileName);
                    using(var Stream=new FileStream(FilePath, FileMode.Create))
                    {
                        await image.CopyToAsync(Stream);
                        SomeImage= FileName;
                    }
                }


                data.ProductName = product.ProductName;
                data.ProductDescription = product.ProductDescription;
                data.Price = product.Price;
                data.Image= SomeImage;
                data.Quandity = product.Quandity;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                if (id == 0 || id == null)
                {
                    return false;
                }
                var ProductId = id;
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync("delete from products where productId={0}", ProductId);
                return rowsAffected > 0;
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
        public async Task<bool> DeleteCategory(int categoryId)
        {
            try 
            {
                if (categoryId == null)
                {
                    return false;
                }
                var CategoryId = categoryId;
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Products WHERE ProductCategoryId = {0}", categoryId);
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync("DELETE FROM Categories WHERE CategoryId = {0}", categoryId);

                return rowsAffected> 0;
            }
            catch(Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }
        public async Task<IEnumerable<ProductDto>> SearchProduct(string SearchItem)
        {
            try
            {
                var Item =  _context.Products.Where(s => s.ProductName.Contains(SearchItem));
                if (Item == null)
                {
                    return new List<ProductDto>();
                }
                var productDtos = Item.Select(p=>
                  new ProductDto
                  {
                      ProductId = p.ProductId,
                      IsAvailable = p.IsAvailable,
                      Image = $"{_configuration["HostUrl:Images"]}/Products/{p.Image}",
                      Price = p.Price,
                      ProductCategoryId = p.ProductCategoryId,
                      ProductDescription = p.ProductDescription,
                      ProductName = p.ProductName,
                      Quandity = p.Quandity
                  });
               

                return productDtos.ToList(); 
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database update error: {innerMessage}");
            }
        }

    }
}
