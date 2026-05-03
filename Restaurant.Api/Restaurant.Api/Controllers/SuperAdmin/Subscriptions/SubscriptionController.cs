using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;

namespace Restaurant.Api.Controllers.SuperAdmin.Subscriptions
{
    [Route("api/v1/super/subscriptions")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IGetAllSubscriptionsService _getAllSubscriptionsService;
        private readonly ICreateSubscriptionService _createSubscriptionService;
        private readonly IUpdateSubscriptionService _updateSubscriptionService;

        public SubscriptionController(
            IGetAllSubscriptionsService getAllSubscriptionsService,
            ICreateSubscriptionService createSubscriptionService,
            IUpdateSubscriptionService updateSubscriptionService)
        {
            _getAllSubscriptionsService = getAllSubscriptionsService;
            _createSubscriptionService = createSubscriptionService;
            _updateSubscriptionService = updateSubscriptionService;
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

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<SubscriptionPlanDto>>> UpdateSubscription(
            int id,
            [FromBody] UpdateSubscriptionDto updateDto)
        {
            var result = await _updateSubscriptionService.UpdateSubscriptionAsync(id, updateDto);

            return StatusCode(result.StatusCode, result);
        }
    }
}
