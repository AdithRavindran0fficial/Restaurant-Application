using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Admin.DTOs;
using Restaurant.Application.Common;
using Restaurant.Application.User.Interfaces.Categories.GetAllCategories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.Api.Controllers.User.Categories
{
    [Route("api/v1/user/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGetAllCategoriesService _getAllCategoriesService;

        public CategoryController(IGetAllCategoriesService getAllCategoriesService)
        {
            _getAllCategoriesService = getAllCategoriesService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAllCategories([FromQuery]string qrToken)
        {
            var result = await _getAllCategoriesService.GetAllCategoriesAsync(qrToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
