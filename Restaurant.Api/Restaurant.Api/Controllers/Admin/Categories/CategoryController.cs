using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Categories.GetAllCategories;
using Restaurant.Application.Admin.Interfaces.Categories.GetCategoryById;
using Restaurant.Application.Common;
using System.Collections.Generic;

namespace Restaurant.Api.Controllers.Admin.Categories
{
    [Route("api/v1/admin/categories")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly IGetAllCategoriesService _getAllCategoriesService;
        private readonly IGetCategoryByIdService _getCategoryByIdService;

        public CategoryController(IGetAllCategoriesService getAllCategoriesService, IGetCategoryByIdService getCategoryByIdService)
        {
            _getAllCategoriesService = getAllCategoriesService;
            _getCategoryByIdService = getCategoryByIdService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAllCategories()
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<List<CategoryDto>>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getAllCategoriesService.GetAllCategoriesAsync(tenantId);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryById(int categoryId)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<CategoryDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _getCategoryByIdService.GetCategoryByIdAsync(tenantId, categoryId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
