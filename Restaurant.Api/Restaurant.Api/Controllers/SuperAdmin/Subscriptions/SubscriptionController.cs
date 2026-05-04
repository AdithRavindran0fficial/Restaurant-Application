using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.SuperAdmin.DTOs;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetAllSubscriptions;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.GetSubscriptionById;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.CreateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.UpdateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeleteSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.ActivateSubscription;
using Restaurant.Application.SuperAdmin.Interfaces.Subscription.DeactivateSubscription;

namespace Restaurant.Api.Controllers.SuperAdmin.Subscriptions
{
    [Route("api/v1/super/subscriptions")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IGetAllSubscriptionsService _getAllSubscriptionsService;
        private readonly IGetSubscriptionByIdService _getSubscriptionByIdService;
        private readonly ICreateSubscriptionService _createSubscriptionService;
        private readonly IUpdateSubscriptionService _updateSubscriptionService;
        private readonly IDeleteSubscriptionService _deleteSubscriptionService;
        private readonly IActivateSubscriptionService _activateSubscriptionService;
        private readonly IDeactivateSubscriptionService _deactivateSubscriptionService;

        public SubscriptionController(
            IGetAllSubscriptionsService getAllSubscriptionsService,
            IGetSubscriptionByIdService getSubscriptionByIdService,
            ICreateSubscriptionService createSubscriptionService,
            IUpdateSubscriptionService updateSubscriptionService,
            IDeleteSubscriptionService deleteSubscriptionService,
            IActivateSubscriptionService activateSubscriptionService,
            IDeactivateSubscriptionService deactivateSubscriptionService)
        {
            _getAllSubscriptionsService = getAllSubscriptionsService;
            _getSubscriptionByIdService = getSubscriptionByIdService;
            _createSubscriptionService = createSubscriptionService;
            _updateSubscriptionService = updateSubscriptionService;
            _deleteSubscriptionService = deleteSubscriptionService;
            _activateSubscriptionService = activateSubscriptionService;
            _deactivateSubscriptionService = deactivateSubscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SubscriptionPlanDto>>>> GetAllSubscriptions()
        {
            var result = await _getAllSubscriptionsService.GetAllSubscriptionsAsync();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SubscriptionPlanDto>>> GetSubscriptionById(int id)
        {
            var result = await _getSubscriptionByIdService.GetSubscriptionByIdAsync(id);

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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteSubscription(int id)
        {
            var result = await _deleteSubscriptionService.DeleteSubscriptionAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateSubscription(int id)
        {
            var result = await _activateSubscriptionService.ActivateSubscriptionAsync(id);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateSubscription(int id)
        {
            var result = await _deactivateSubscriptionService.DeactivateSubscriptionAsync(id);

            return StatusCode(result.StatusCode, result);
        }
    }
}
