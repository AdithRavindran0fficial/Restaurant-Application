using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Common;
using Restaurant.Application.User.DTOs;
using Restaurant.Application.User.Interfaces.Session.CreateSession;

namespace Restaurant.Api.Controllers.User.Sessions
{
    [ApiController]
    [Route("api/v1/user/session")]
    public class SessionController : ControllerBase
    {
        private readonly ICreateSessionService _createSessionService;

        public SessionController(ICreateSessionService createSessionService)
        {
            _createSessionService = createSessionService;
        }

        [HttpPost("{qrToken}")]
        public async Task<ActionResult<ApiResponse<SessionDTO>>> CreateSession(
            [FromRoute] string qrToken)
        {
            var result = await _createSessionService
                .CreateSessionAsync(qrToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}