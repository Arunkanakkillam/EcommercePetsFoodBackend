using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models.Orders;
using EcommercePetsFoodBackend.Db_Context;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace EcommercePetsFoodBackend.Services.OrderServices
{
    public class OrderService:IOrderService
    {
        private readonly EcomContext _context;
        private readonly IConfiguration _configuration;
       
        public OrderService(EcomContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        
        }
        public async Task<string>OrderCreate(long Price)
        {
            try 
            {
                if (Price <= 0 ) 
                {
                    throw new Exception("price is not valid");
                }
                Dictionary<string, object> data = new Dictionary<string, object>();
                string TransactionId = Guid.NewGuid().ToString();
                data.Add("amount",Convert.ToDecimal(Price)*100);
                data.Add("currency", "INR");
                data.Add("receipt", TransactionId);
                string _key = _configuration["Razorpay:Key"];
                string _secret = _configuration["Razorpay:Secret"];
                RazorpayClient client=new RazorpayClient(_key, _secret);
                Razorpay.Api.Order order=client.Order.Create(data);
                var OrderId = order["id"].ToString();
                return OrderId;
            }
            catch(Exception ex) 
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Order creation failed: {innerMessage}");
            }
         
        }


        public async Task<bool> CreateOrder(InputOrderDto UserCredential)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cartItems = await _context.Customers
                    .Include(u => u.cart)
                    .ThenInclude(ci => ci.Items)
                    .ThenInclude(p => p.Product)
                    .SingleOrDefaultAsync(u=>u.Id==UserCredential.User_Id);
                if(cartItems == null)
                {
                    throw new Exception("user doesn't have a cart");
                }
                var UserHaveOrder = await GetOrCreateOrderAsync(UserCredential.User_Id);

                var orderItemList = cartItems.cart.Items.Select(ci => new OrderItem
                {
                    OrderId = UserHaveOrder.OrderId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price,
                    TotalPrice = ci.Product.Price * ci.Quantity,
                    DeliveryAddress = UserCredential.DeliveryAddres,
                    OrderDate = DateTime.Now.Date,
                    Phone = UserCredential.Phone,
                    Email = UserCredential.Email,
                    Name = UserCredential.Name

                });
                await _context.OrderItems.AddRangeAsync(orderItemList);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex) 
            {
                await transaction.RollbackAsync();
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Order creation failed: {innerMessage}");
            }
        }


        public async Task<bool> Payment(RazorPayDto paymentDto)
        {
           
                if (paymentDto is null||string.IsNullOrEmpty(paymentDto.razor_payment_id)||string.IsNullOrEmpty(paymentDto.razor_order_id)
                    ||string.IsNullOrEmpty(paymentDto.razor_signature))
                {
                    throw new ArgumentException("Payment details are invalid.");
                }
            try
            {

                RazorpayClient client = new RazorpayClient(_configuration["Razorpay:Key"], _configuration["Razorpay:Secret"]);
                var attribute = new Dictionary<string, string>
                {
                    ["razor_payment_id"] = paymentDto.razor_payment_id,
                    ["razor_order_id"] = paymentDto.razor_order_id,
                    ["razor_signature"] = paymentDto.razor_signature
                };
                
                Utils.verifyPaymentSignature(attribute);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Payment verification failed: " +
                    $"{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<IEnumerable<OutPutOrderDto>> CustomersOrders(int id)
        {
            try
            {
                var order = await _context.Customers
                    .Include(o => o.orders)
                    .ThenInclude(oi => oi.orderItems)
                    .ThenInclude(p => p.Product)
                    .SingleOrDefaultAsync(u => u.Id == id);
                if ( order?.orders == null)
                {
                    return Enumerable.Empty<OutPutOrderDto>();
                }
                var orderList = order.orders.SelectMany(o => o.orderItems.Select(oi => new OutPutOrderDto
                {
                    Id = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Total = oi.TotalPrice,
                    UserId = oi.OrderId,
                    image = oi.Product.Image
                }))
                    .ToList();
                return orderList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get customer orders: " +
                    $"{ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<IEnumerable<OutPutOrderDto>> GetAllOrders()
        {
            try
            {
                var order = await _context.Customers
                    .Include(o => o.orders)
                    .ThenInclude(oi => oi.orderItems)
                    .ThenInclude(p => p.Product)
                    .ToListAsync();
                if(order?.Count == 0)
                {
                    return Enumerable.Empty<OutPutOrderDto>();
                }
                var orderList = order.SelectMany(customer => customer.orders.SelectMany(order => order.orderItems.Select(orderItem => new OutPutOrderDto
                {
                    Id = order.OrderId,
                    Quantity = orderItem.Quantity,
                    ProductId = orderItem.ProductId,
                    Price = orderItem.Price,
                    Total = orderItem.TotalPrice,
                    UserId = customer.Id,
                    image = orderItem.Product.Image

                }))).ToList();
                return orderList;   
            }
            catch (Exception ex)
            { 
                throw new InvalidOperationException($"Failed to GetAllOrders: {ex.Message}" );
            }
        }

        public async Task<bool> DeleteAllOrders(int id)
        {
            try
            {
                var order=await _context.Orders.Where(o=>o.CustomerId==id).ToListAsync();
                if (order == null)
                {
                    return false;
                }
                 _context.Orders.RemoveRange(order);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex) 
            {
                throw new InvalidOperationException($"failed to delete orders: {ex.Message}");
            }
        }
        public async Task<Revenuedto> TotalRevenue()
        {
          
            try
            {
                var revenue = await _context.OrderItems
                        .SumAsync(i => i.TotalPrice);
                return new Revenuedto
                {
                    Total= revenue
                };
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException(ex.Message);
            }
        }


        private async Task<Data.Models.Orders.Order> GetOrCreateOrderAsync(int userId)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(o => o.CustomerId == userId);
            if (order == null)
            {
                order= new Data.Models.Orders.Order { CustomerId = userId };
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            return order;
        }
    }
}
