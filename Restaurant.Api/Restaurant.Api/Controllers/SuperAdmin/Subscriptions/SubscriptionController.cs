using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;

namespace Restaurant.Api.Controllers.SuperAdmin.Subscriptions
{
    [Route("api/v1/super/subscriptions")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IGetAllSubscriptionsService _getAllSubscriptionsService;

        public SubscriptionController(IGetAllSubscriptionsService getAllSubscriptionsService)
        {
            _getAllSubscriptionsService = getAllSubscriptionsService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SubscriptionPlanDto>>>> GetAllSubscriptions()
        {
            var result = await _getAllSubscriptionsService.GetAllSubscriptionsAsync();

            return StatusCode(result.StatusCode, result);
        }
    }
}
