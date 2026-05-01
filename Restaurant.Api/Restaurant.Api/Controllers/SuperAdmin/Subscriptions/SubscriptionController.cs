using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;

namespace Restaurant.Api.Controllers.SuperAdmin.Subscriptions
{
    [Route("api/v1/super/subscriptions")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IGetAllSubscriptionsService _getAllSubscriptionsService;
        private readonly ICreateSubscriptionService _createSubscriptionService;

        public SubscriptionController(
            IGetAllSubscriptionsService getAllSubscriptionsService,
            ICreateSubscriptionService createSubscriptionService)
        {
            _getAllSubscriptionsService = getAllSubscriptionsService;
            _createSubscriptionService = createSubscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SubscriptionPlanDto>>>> GetAllSubscriptions()
        {
            var result = await _getAllSubscriptionsService.GetAllSubscriptionsAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SubscriptionPlanDto>>> CreateSubscription(
            [FromBody] CreateSubscriptionDto createDto)
        {
            var result = await _createSubscriptionService.CreateSubscriptionAsync(createDto);

            return StatusCode(result.StatusCode, result);
        }
    }
}
