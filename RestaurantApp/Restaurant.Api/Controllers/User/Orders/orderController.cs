using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.User.DTOs.OrderDTOs;
using Restaurant.Application.User.Interfaces.Order;
using Restaurant.Application.User.Interfaces.Orders;

namespace Restaurant.Api.Controllers.User.Orders
{
    [Route("api/v1/user/order")]
    [ApiController]
    public class orderController : ControllerBase
    {

        private readonly ICreateOrderService createOrderService;
        private readonly IActiveOrdersService activeOrdersService;
        public orderController(ICreateOrderService createOrder,IActiveOrdersService activeOrders)
        {
            createOrderService = createOrder;
            activeOrdersService = activeOrders;
        }
       
        [HttpPost]
        public async Task<IActionResult> OrderAsync(CreateOrderDTO createOrderDTO)
        {
            var  response = await createOrderService.CreateOrderAsync(createOrderDTO);
            return StatusCode(response.StatusCode,response);
        }

        [HttpPost("active")]
        public async Task<IActionResult> GetActiveOrders(ActiveOrderRequestDTO activeOrderRequestDTO)
        {
            var result = await activeOrdersService.GetActiveOrders(activeOrderRequestDTO);
            return StatusCode(result.StatusCode, result);
        }
    }
}
