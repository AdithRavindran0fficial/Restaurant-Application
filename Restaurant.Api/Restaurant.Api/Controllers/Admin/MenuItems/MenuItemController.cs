using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetAllMenuItems;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemById;
using Restaurant.Application.Admin.Interfaces.MenuItems.GetMenuItemsByCategoryId;
using Restaurant.Application.Admin.Interfaces.MenuItems.CreateMenuItem;
using Restaurant.Application.Admin.Interfaces.MenuItems.UpdateMenuItem;
using Restaurant.Application.Admin.Interfaces.MenuItems.DeleteMenuItem;
using Restaurant.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers.Admin.MenuItems
{
    [Route("api/v1/admin/menu-items")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MenuItemController : ControllerBase
    {
        private readonly IGetAllMenuItemsService _getAllMenuItemsService;
        private readonly IGetMenuItemByIdService _getMenuItemByIdService;
        private readonly IGetMenuItemsByCategoryIdService _getMenuItemsByCategoryIdService;
        private readonly ICreateMenuItemService _createMenuItemService;
        private readonly IUpdateMenuItemService _updateMenuItemService;
        private readonly IDeleteMenuItemService _deleteMenuItemService;

        public MenuItemController(
            IGetAllMenuItemsService getAllMenuItemsService,
            IGetMenuItemByIdService getMenuItemByIdService,
            IGetMenuItemsByCategoryIdService getMenuItemsByCategoryIdService,
            ICreateMenuItemService createMenuItemService,
            IUpdateMenuItemService updateMenuItemService,
            IDeleteMenuItemService deleteMenuItemService)
        {
            _getAllMenuItemsService = getAllMenuItemsService;
            _getMenuItemByIdService = getMenuItemByIdService;
            _getMenuItemsByCategoryIdService = getMenuItemsByCategoryIdService;
            _createMenuItemService = createMenuItemService;
            _updateMenuItemService = updateMenuItemService;
            _deleteMenuItemService = deleteMenuItemService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MenuItemDto>>>> GetAllMenuItems()
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<List<MenuItemDto>>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getAllMenuItemsService.GetAllMenuItemsAsync(tenantId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{menuItemId}")]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> GetMenuItemById(int menuItemId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<MenuItemDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getMenuItemByIdService.GetMenuItemByIdAsync(tenantId, menuItemId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<MenuItemDto>>>> GetMenuItemsByCategoryId(int categoryId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<List<MenuItemDto>>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getMenuItemsByCategoryIdService.GetMenuItemsByCategoryIdAsync(tenantId, categoryId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> CreateMenuItem([FromForm] CreateMenuItemDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<MenuItemDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _createMenuItemService.CreateMenuItemAsync(tenantId, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{menuItemId}")]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> UpdateMenuItem(int menuItemId, [FromForm] UpdateMenuItemDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<MenuItemDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _updateMenuItemService.UpdateMenuItemAsync(tenantId, menuItemId, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{menuItemId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMenuItem(int menuItemId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<bool>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _deleteMenuItemService.DeleteMenuItemAsync(tenantId, menuItemId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
