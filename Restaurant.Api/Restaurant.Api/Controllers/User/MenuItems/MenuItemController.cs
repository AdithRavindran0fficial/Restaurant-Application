using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemById;
using Restaurant.Application.User.Interfaces.MenuItems.GetMenuItemsByCategoryId;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers.User.MenuItems
{
    [Route("api/v1/user/menu-items")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IGetMenuItemByIdService _getMenuItemByIdService;
        private readonly IGetMenuItemsByCategoryIdService _getMenuItemsByCategoryIdService;

        public MenuItemController(
            IGetMenuItemByIdService getMenuItemByIdService,
            IGetMenuItemsByCategoryIdService getMenuItemsByCategoryIdService)
        {
            _getMenuItemByIdService = getMenuItemByIdService;
            _getMenuItemsByCategoryIdService = getMenuItemsByCategoryIdService;
        }

        [HttpGet("{menuItemId}")]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> GetMenuItemById([FromRoute] int menuItemId, [FromQuery] string qrToken)
        {
            var result = await _getMenuItemByIdService.GetMenuItemByIdAsync(qrToken, menuItemId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<MenuItemDto>>>> GetMenuItemsByCategoryId([FromRoute] int categoryId, [FromQuery] string qrToken)
        {
            var result = await _getMenuItemsByCategoryIdService.GetMenuItemsByCategoryIdAsync(qrToken, categoryId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
