//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Diagnostics;

//namespace EcommercePetsFoodBackend.Services.WishlistServices
//{
//    public class CustomFilter:IAsyncActionFilter
//    {
//        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            var stopwatch = Stopwatch.StartNew();

//            var resultContext = await next();

//            stopwatch.Stop();
//            var executionTime = stopwatch.ElapsedMilliseconds;

//            Console.WriteLine($"Action executed in {executionTime} ms");
//        }
//    }
//  }
