using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.User.DTOs.OrderDTOs;

namespace Restaurant.Api.Controllers.User.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class orderController : ControllerBase
    {
        public orderController()
        {
            
        }
        //[HttpPost]
        //public Task<IActionResult>OrderAsync(CreateOrderDTO createOrderDTO)
        //{
        //    //var response
        //}
    }
}
