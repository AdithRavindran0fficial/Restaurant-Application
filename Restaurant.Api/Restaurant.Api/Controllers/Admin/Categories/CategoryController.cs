using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Admin.Interfaces.Categories.GetAllCategories;
using Restaurant.Application.Admin.Interfaces.Categories.GetCategoryById;
using Restaurant.Application.Admin.Interfaces.Categories.CreateCategory;
using Restaurant.Application.Admin.Interfaces.Categories.UpdateCategory;
using Restaurant.Application.Admin.DTOs;
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
        private readonly ICreateCategoryService _createCategoryService;
        private readonly IUpdateCategoryService _updateCategoryService;

        public CategoryController(
            IGetAllCategoriesService getAllCategoriesService,
            IGetCategoryByIdService getCategoryByIdService,
            ICreateCategoryService createCategoryService,
            IUpdateCategoryService updateCategoryService)
        {
            _getAllCategoriesService = getAllCategoriesService;
            _getCategoryByIdService = getCategoryByIdService;
            _createCategoryService = createCategoryService;
            _updateCategoryService = updateCategoryService;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<CategoryDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _createCategoryService.CreateCategoryAsync(tenantId, dto);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(int categoryId, [FromBody] UpdateCategoryDto dto)
        {
            var tenantIdClaim = User.FindFirst("tenantId")?.Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim) || !int.TryParse(tenantIdClaim, out int tenantId))
            {
                return Unauthorized(ApiResponse<CategoryDto>.UnauthorizedResponse(
                    "Tenant information missing from token"));
            }

            var result = await _updateCategoryService.UpdateCategoryAsync(tenantId, categoryId, dto);

            return StatusCode(result.StatusCode, result);
        }
    }
}
